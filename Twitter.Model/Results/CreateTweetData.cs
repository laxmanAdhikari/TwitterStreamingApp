using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Model.Results
{
    public class CreateTweetData
    {
        public Entities.Tweet Tweet { get; set; }

        public CreateTweetData(Entities.Tweet tweet)
        {
            Tweet = tweet;
        }
    }
}
