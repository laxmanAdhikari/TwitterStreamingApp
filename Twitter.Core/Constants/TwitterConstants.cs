using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Core.Constants
{
    public class TwitterConstants
    {
        public const string EXPANSIONS_AND_FIELD_QUERY = "expansions=author_id,in_reply_to_user_id,attachments.media_keys&user.fields=profile_image_url,username&tweet.fields=created_at,public_metrics,entities,referenced_tweets&media.fields=media_key,url,type,public_metrics";

        public const string SAMPLE_STREAM_URL = "TwitterStreamApi";

        public const string BEARER_TOKEN = "BearerToken";

        public const string CONSUMER_KEY = "ConsumerKey";

        public const string CONSUMER_SECRET = "ConsumerSecret";

        public const string DATABASE_CONNECTION = "TwitterStreamDbContext";
    }
}
