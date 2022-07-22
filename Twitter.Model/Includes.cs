using Newtonsoft.Json;

namespace Twitter.Model
{
    public class Includes
    {
        [JsonProperty("users")]
        public List<User>? Users { get; set; }

        [JsonProperty("media")]
        public List<Media>? Media { get; set; }
    }
}
