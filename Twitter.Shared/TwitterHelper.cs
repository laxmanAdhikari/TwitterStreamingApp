using Tweetinvi;

namespace Twitter.Shared
{
    public class TwitterHelper
    {
        public TwitterClient twitterClient { get; private set; }

        private const string TWITTERAPIKEY = "TwitterApiKey";
        private const string TWITTERAPISECRET = "TwitterApiSecret";
        private const string TWITTERACCESSTOKEN = "TwitterAccessToken";
        private const string TWITTERACCESSTOKENSECRET = "TwitterAccessTokenSecret";

        public TwitterHelper()
        {
            DotNetEnv.Env.Load();
            twitterClient = new TwitterClient(Environment.GetEnvironmentVariable(TWITTERAPIKEY)
                    , Environment.GetEnvironmentVariable(TWITTERAPISECRET),
                    Environment.GetEnvironmentVariable(TWITTERACCESSTOKEN),
                    Environment.GetEnvironmentVariable(TWITTERACCESSTOKENSECRET));
        }
    }
}