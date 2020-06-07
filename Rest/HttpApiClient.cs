using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gracie.Http
{
    public class HttpApiClient
    {
        private readonly HttpClient httpClient;
        private readonly IRateLimitBucketRepo rateLimitBucketRepo;
        public readonly string AuthHeader;
        private byte[] authHash;

        public HttpApiClient(HttpClient httpClient, TokenType tokenType, string token, IRateLimitBucketRepo rateLimitBucketRepo)
        {
            this.httpClient = httpClient;
            this.rateLimitBucketRepo = rateLimitBucketRepo;
            AuthHeader = tokenType.StringValue() + " " + token;
        }

        public byte[] GetAuthHash()
        {
            if (authHash is null)
            {
                var hashAlgo = SHA256.Create();
                authHash = hashAlgo.ComputeHash(Encoding.UTF8.GetBytes(AuthHeader));
            }
            return authHash;
        }

        public static Uri BuildUri(string baseUri, string path)
        {
            var builder = new UriBuilder(baseUri);
            builder.Path += path;
            return builder.Uri;
        }

        private async Task RateLimit(byte[] operation, CancellationToken cancellationToken = default)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                var globalRateLimit = await rateLimitBucketRepo.GlobalRateLimited();
                if (globalRateLimit.HasValue)
                {
                    await Task.Delay(globalRateLimit.Value, cancellationToken);
                    break;
                }

                if (cancellationToken.IsCancellationRequested)
                    return;

                var (canExecuteOperation, waitTimeOperation) = await rateLimitBucketRepo.ExecuteAgainstBucket(operation);

                if (canExecuteOperation)
                {
                    break;
                }
                await Task.Delay(waitTimeOperation, cancellationToken);
            }
        }

        public async Task<HttpResponseMessage> SendRequest(byte[] operation, HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            await RateLimit(operation, cancellationToken);

            if (cancellationToken.IsCancellationRequested)
                return null;

            request.Headers.Add("Authorization", AuthHeader);
            request.Headers.Add("X-RateLimit-Precision", "millisecond");

            var result = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            var global = result.Headers.GetValues("X-RateLimit-Global").FirstOrDefault();
            var retryAfter = result.Headers.GetValues("Retry-After").FirstOrDefault();
            var limit = result.Headers.GetValues("X-RateLimit-Limit").FirstOrDefault();
            var remaining = result.Headers.GetValues("X-RateLimit-Remaining").FirstOrDefault();
            var resetAfter = result.Headers.GetValues("X-RateLimit-Reset-After").FirstOrDefault();
            var bucketName = result.Headers.GetValues("X-RateLimit-Bucket").FirstOrDefault();

            if (global != null && global.ToLowerInvariant().Equals("true"))
            {
                await rateLimitBucketRepo.SetGlocalRateLimited(Convert.ToInt32(retryAfter));
            }
            else if (bucketName != null)
            {
                var resetAfterDouble = Convert.ToDouble(resetAfter);
                var resetAfterDt = DateTime.Now.AddSeconds(resetAfterDouble);
                var limitInt = Convert.ToInt32(limit);
                var remainingInt = Convert.ToInt32(remaining);
                await rateLimitBucketRepo.SetBucketForOperation(operation, bucketName, limitInt, remainingInt, resetAfterDt);
            }
            return result;
        }
    }
}
