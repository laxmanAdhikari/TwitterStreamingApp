using Microsoft.EntityFrameworkCore;
using Twitter.Model.Entities;
using TwitterStreamApi.Services.Base;
using TwitterStreamApi.Data;
using Twitter.Core.Extentions;

namespace TwitterStreamApi.Services
{
    public class AuthorService : BaseService<Author>, IAuthorService
    {
        public AuthorService(TwitterDbContext twitterDbContext, ILogger<AuthorService> logger) : base(twitterDbContext, logger) { }

        public async Task<Author> GetByTwitterAuthorAsync(string twitterAuthorId)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Method", "GetByTwitterAuthorAsync");
            parameters.Add("Twitter API Author Id", twitterAuthorId);

            try
            {
                // Get the record based on the Twitter API author identifier. Returns null if not found.
                return await _twitterDbContext.AuthorEntities.FirstOrDefaultAsync(author => author.TwwitterAuthorId == twitterAuthorId);
            }
            catch (Exception exception)
            {
                // Log any exceptions.
                _logger.LogWithParameters(LogLevel.Error, exception, "Unable to complete method due to an exception", parameters);
                throw;
            }
        }

    }
}
