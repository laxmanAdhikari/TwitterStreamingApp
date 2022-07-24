using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Core.Exceptions
{
    public class TwitterException: Exception
    {
        public DateTimeOffset? XRateLimitResetDate { get; init; }

        public TwitterException(string message) : base(message) { }

        public TwitterException(string message, Exception innerException) : base(message, innerException) { }

        
        public TwitterException(string message, Exception innerException, int? xRateLimitSet) : this(message, innerException)
        {
            if (xRateLimitSet.HasValue)
            {
                XRateLimitResetDate = DateTimeOffset.FromUnixTimeSeconds(xRateLimitSet.Value);
            }
            
        }

        public TwitterException(string message, int? xRateLimitReset) : this(message, null, xRateLimitReset)
        {
        }
    }
}

    

