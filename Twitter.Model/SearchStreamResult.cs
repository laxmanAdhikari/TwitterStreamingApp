using Newtonsoft.Json;

namespace Twitter.Model
{
    public class SearchStreamResult
    {
        [JsonProperty("data")]
        public Tweet? Tweet { get; set; }

        [JsonProperty("includes")]
        public Includes? Includes { get; set; }

    }
}
