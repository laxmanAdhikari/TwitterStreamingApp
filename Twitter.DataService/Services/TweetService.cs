using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Twitter.Core.Extentions;
using Twitter.Model.Entities;
using Twitter.Service.Data;
using Twitter.Service.Services.Base;

namespace Twitter.Service.Services
{

    public class TweetService : BaseService<Tweet>, ITweetService
    {
        protected new readonly DbContextOptions<TwitterDbContext> _twitterDbContext;

        public TweetService(DbContextOptions<TwitterDbContext> twitterDbContext, ILogger<TweetService> logger) : base(twitterDbContext, logger)
        {
            _twitterDbContext = twitterDbContext;
        }

        public List<string>? HashTagCollection { get; set; }
       

        public async Task<Tweet> GetByTwitterTweetAsync(string twitterTweetId)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Method", "GetByTwitterTweetAsync");
            parameters.Add("Twitter Tweet ID", twitterTweetId);

            try
            {
                using var db = new TwitterDbContext(_twitterDbContext) ;
                return await db.TweetEntities.FirstOrDefaultAsync(tweet => tweet.TweeterTweetId == twitterTweetId);
            }
            catch (Exception exception)
            {
                _logger.LogWithParameters(LogLevel.Error, exception, exception.Message, parameters);
                throw;
            }
        }
        public async Task<int> GetCount()

        {
            using var db = new TwitterDbContext(_twitterDbContext);
            return await db.TweetEntities.CountAsync().ConfigureAwait(false);
        }

    }
}