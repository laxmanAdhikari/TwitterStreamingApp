using Newtonsoft.Json;

namespace Twitter.Model
{
    public class Tweet
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("text")]
        public string? Text { get; set; }

        //[JsonProperty("attachments")]
        //public Attachment? Attachments { get; set; }

        [JsonProperty("author_id")]
        public string? AuthorId { get; set; }

        [JsonProperty("created_at")]
        public string? Created { get; set; }

        [JsonProperty("referenced_tweets")]
        public List<ReferenceTweet>? ReferenceTweets { get; set; }


    }
}
