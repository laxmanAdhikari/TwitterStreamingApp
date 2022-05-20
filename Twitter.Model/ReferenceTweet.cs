using Newtonsoft.Json;

namespace Twitter.Model
{
    public class ReferenceTweet
    {
        [JsonProperty("type")]
        public string? ReferenceType { get; set; }

        [JsonProperty("string")]
        public string? Id { get; set; }
    }
}
