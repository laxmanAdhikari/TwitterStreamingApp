using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Model
{
    public class User
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("idprofile_image_url")]
        public string? ProfileImageUrl { get; set; }

        [JsonProperty("username")]
        public string? UserName { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }
    }
}
