using Newtonsoft.Json;

namespace Twitter.Model
{
    public class Attachment
    {
        [JsonProperty("media_keys")]
        public List<string>? MediaKeys { get; set; }
    }
}
