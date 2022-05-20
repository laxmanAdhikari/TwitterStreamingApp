using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Model.Entities
{
    [Table("Tweet", Schema ="JH.TwitterStreamingApp")]
    public class Tweet : Base
    {

        public int AuthorId { get; set; }
        public int TweetId { get; set; }

        public string? Content { get; set; }

        public DateTimeOffset TwitterPublished { get; set; }

    }
}
