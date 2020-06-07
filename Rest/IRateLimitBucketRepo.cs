using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gracie.Http
{
    public interface IRateLimitBucketRepo
    {
        /// <summary>
        /// Attempt to execute against a rate limit bucket
        /// if it returns true, you can execute and the ratelimit bucket has been decreased by 1
        /// if it returns false, waitTime will be set with the number of milliseconds you need to wait before trying again
        /// </summary>
        Task<(bool, int)> ExecuteAgainstBucket(byte[] operation);
        Task SetBucketForOperation(byte[] operation, string bucketId, int max, int remaining, DateTime reset);
        Task<int?> GlobalRateLimited();
        Task SetGlocalRateLimited(int milliseconds);
    }
}
