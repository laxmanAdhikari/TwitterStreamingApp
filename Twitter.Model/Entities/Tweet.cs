using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace Twitter.Model.Entities
{
    [Table("Tweet", Schema ="JH.TwitterStreamingApp")]
    public class Tweet : Base
    {

        public int AuthorId { get; set; }
        public string TweeterTweetId { get; set; }

        public string Content { get; set; }

        public DateTimeOffset TwitterPublished { get; set; }

        public Author Author { get; set; }


        public static void OnModelCreating(EntityTypeBuilder<Tweet> builder)
        {
            OnModelCreating<Tweet>(builder);
            builder.Property(tweet => tweet.TweeterTweetId).HasMaxLength(50);
            builder.Property(tweet => tweet.Content).HasMaxLength(1000);
        }

    }
}
