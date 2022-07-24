using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Model.Entities
{
    [Table("HashTag", Schema = "JH.TwitterStreamingApp")]
    public class HashTag : Base
    {
        public string AuthorId { get; set; }
        public string TweeterTweetId { get; set; }
        public string HashTagName { get; set; }

        public static void OnModelCreating(EntityTypeBuilder<Entities.HashTag> builder)
        {
            OnModelCreating<Entities.HashTag>(builder);

        }
    }
}
