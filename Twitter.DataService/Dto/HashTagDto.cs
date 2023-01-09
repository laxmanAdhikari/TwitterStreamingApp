using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Service.Dto
{
    public class HashTagDto
    {
        public string AuthorId { get; set; } = string.Empty;
        public string TweeterTweetId { get; set; } = string.Empty;
        public string HashTagName { get; set; } = string.Empty; 
    }
}
