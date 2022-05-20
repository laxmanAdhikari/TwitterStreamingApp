using Newtonsoft.Json;

namespace Twitter.Model
{
    public class Media
    {
        [JsonProperty("media_key")]
        public string? MediaKey { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("url")]
        public string? Url { get; set; }
    }
}
