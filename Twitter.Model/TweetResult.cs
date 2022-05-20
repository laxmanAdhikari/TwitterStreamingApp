using Newtonsoft.Json;

namespace Twitter.Model
{
    public class TweetResult
    {
        [JsonProperty("data")]
        public List<Tweet>? Tweets { get; set; }

    }
}
