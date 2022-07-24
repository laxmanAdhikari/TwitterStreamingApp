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

    }
}