using System;
using System.Collections.Generic;
using System.Text;

namespace Gracie.Http
{
    public class RateLimitBucketOld
    {
        public string Name { get; set; }

        public int? Limit { get; set; }

        public int? Remaining { get; set; }

        public DateTime? Reset { get; set; }

        public readonly object Lock = new object();

        public static int? ParseLimit(string limit)
        {
            if (limit != null && int.TryParse(limit, out var cast))
            {
                return cast;
            }
            return null;
        }

        public static int? ParseRemaining(string remaining)
        {
            if (remaining != null && int.TryParse(remaining, out var cast))
            {
                return cast;
            }
            return null;
        }

        public static DateTime? ParseReset(string resetAfter)
        {
            if (resetAfter != null && double.TryParse(resetAfter, out var cast))
            {
                return DateTime.Now.AddSeconds(cast);
            }
            return null;
        }
    }
}
