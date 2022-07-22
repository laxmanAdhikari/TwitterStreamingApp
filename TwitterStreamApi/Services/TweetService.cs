using Microsoft.EntityFrameworkCore;
using TwitterStreamApi.Services.Base;
using Twitter.Model.Entities;
using TwitterStreamApi.Services;
using TwitterStreamApi.Data;
using Twitter.Core.Extentions;
using System.Text.RegularExpressions;

namespace TwitterStreamApi.Services
{

    public class TweetService : BaseService<Tweet>, ITweetService
    {

        public TweetService(TwitterDbContext twitterDbContext, ILogger<TweetService> logger) : base(twitterDbContext, logger) { }
        
        public List<string> HashTagCollection { get; set; }

        public async Task<Tweet> GetByTwitterTweetAsync(string twitterTweetId)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Method", "GetByTwitterTweetAsync");
            parameters.Add("Twitter Tweet ID", twitterTweetId);

            try
            {
                return await _twitterDbContext.TweetEntities.FirstOrDefaultAsync(tweet => tweet.TweeterTweetId == twitterTweetId);
            }
            catch (Exception exception)
            {
                _logger.LogWithParameters(LogLevel.Error, exception, exception.Message, parameters);
                throw;
            }
        }

        public async Task<int> GetCount()
        {
            return await _twitterDbContext.TweetEntities.CountAsync().ConfigureAwait(false);
        }

        public Task<List<string>> GetHashTags(int topNthvalue)
        {

            int batchNumber = 0;

            HashTagCollection = new List<string>();

            List<string> hashTags = new List<string>();

            do
            {
                var tweets = _twitterDbContext.TweetEntities.OrderByDescending(t => t.TwitterPublished).OrderByDescending(t => t.Id).Skip(batchNumber * topNthvalue).Take(topNthvalue);

                foreach (var tweet in tweets)
                {
                    if (HashTagCollection.Count == topNthvalue)
                        return Task.FromResult(HashTagCollection);

                    CollectHashTags(tweet.Content);
                }

                if (HashTagCollection.Count < topNthvalue)
                {
                    batchNumber = batchNumber + 1;
                }
            } while (HashTagCollection.Count <= topNthvalue);

            return  Task.FromResult(HashTagCollection);
        }

        internal void CollectHashTags(string tweet)
        {
            
            var regex = new Regex(@"#\w+");
            var matches = regex.Matches(tweet);

            foreach (var match in matches)
            {
                if (!HashTagCollection.Contains(match) && HashTagCollection.Count < 10)
                {
                    HashTagCollection.Add(match.ToString());
                }
            }
        }
    }
}