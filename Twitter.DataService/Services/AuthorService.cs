using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Twitter.Core.Extentions;
using Twitter.Model.Entities;
using Twitter.Service.Data;
using Twitter.Service.Services.Base;

namespace Twitter.Service.Services
{
    public class AuthorService : BaseService<Author>, IAuthorService
    {
        public AuthorService(DbContextOptions<TwitterDbContext> twitterDbContext, ILogger<AuthorService> logger) : base(twitterDbContext, logger) { }

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
