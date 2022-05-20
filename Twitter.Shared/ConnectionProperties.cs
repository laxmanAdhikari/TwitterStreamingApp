using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Shared
{
    public class ConnectionProperties
    {
        public string GetTwitterApiKey { get; init; } = string.Empty;
        public string TwitterApiSecret { get; init; } = string.Empty;
        public string TwitterAccessToken { get; init; } = string.Empty;
        public string TwitterAccessTokenSecret { get; init; } = string.Empty;

    }
}
