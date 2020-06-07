using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gracie.Http
{
    public class DictionaryRateLimitBucketRepo : IRateLimitBucketRepo
    {
        private Dictionary<string, string> operationBucketIds = new Dictionary<string, string>();
        private Dictionary<string, RateLimitBucket> buckets = new Dictionary<string, RateLimitBucket>();

        private DateTime? globalRateLimitEnd = null;
        private readonly object globalRateLimitLock = new object();

        public Task<(bool, int)> ExecuteAgainstBucket(byte[] operation)
        {
            var operationBase64 = Convert.ToBase64String(operation);
            string bucketId;
            if (!operationBucketIds.TryGetValue(operationBase64, out bucketId))
            {
                // we have not performed this operation before, we can't know if it has been ratelimited
                return Task.FromResult((true, 0));
            }
            RateLimitBucket bucket;
            // if a bucketId is set so should be a rateLimit bucket
            if (!buckets.TryGetValue(bucketId, out bucket))
            {
                throw new Exception();
            }
            // Prevent other threads from modifying this bucket before we finish
            lock (bucket.Lock)
            {
                if (bucket.Remaining > 0)
                {
                    bucket.Remaining -= 1;
                    Task.FromResult((true, 0));
                }
                if (DateTime.Now > bucket.Reset)
                {
                    bucket.Remaining = bucket.Max - 1;
                    Task.FromResult((true, 0));
                }
                var waitTime = Convert.ToInt32(bucket.Reset.Subtract(DateTime.Now).TotalMilliseconds);
                return Task.FromResult((false, waitTime));
            }
        }

        public Task<int?> GlobalRateLimited()
        {
            if (!globalRateLimitEnd.HasValue)
            {
                return Task.FromResult((int?)null);
            }
            lock (globalRateLimitLock)
            {
                if (globalRateLimitEnd.Value < DateTime.Now)
                {
                    globalRateLimitEnd = null;
                    return Task.FromResult((int?)null);
                }
                else
                {
                    int? result = Convert.ToInt32(globalRateLimitEnd.Value.Subtract(DateTime.Now).TotalMilliseconds);
                    return Task.FromResult(result);
                }
            }
        }

        public Task SetBucketForOperation(byte[] operation, string bucketId, int max, int remaining, DateTime reset)
        {
            var bucket = new RateLimitBucket
            {
                Reset = reset,
                Max = max,
                Remaining = remaining
            };
            buckets[bucketId] = bucket;
            operationBucketIds[Convert.ToBase64String(operation)] = bucketId;
            return Task.CompletedTask;
        }

        public Task SetGlocalRateLimited(int milliseconds)
        {
            globalRateLimitEnd = DateTime.Now.AddMilliseconds(milliseconds);
            return Task.CompletedTask;
        }
    }

    public class RateLimitBucket
    {
        public int Max { get; set; }
        public int Remaining { get; set; }
        public DateTime Reset { get; set; }
        public object Lock { get; } = new object();
    }
}
