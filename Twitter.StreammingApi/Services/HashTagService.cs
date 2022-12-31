using Microsoft.EntityFrameworkCore;
using Twitter.Model.Entities;
using TwitterStreamApi.Services.Base;
using TwitterStreamApi.Data;
using Twitter.Core.Extentions;
using Twitter.StreammingApi.Pagination;

namespace TwitterStreamApi.Services
{
    public class HashTagService : BaseService<HashTag>, IHashTagService
    {
        public HashTagService(TwitterDbContext twitterDbContext, ILogger<HashTagService> logger) : base(twitterDbContext, logger) { }

        public List<string> HashTagCollection { get; set; }

        public async Task<HashTag> GetByTwitterHashTagAsync(string twitterTweetId)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Method", "GetByTwitterHashTagAsync");
            parameters.Add("Twitter API Tweet Id", twitterTweetId);

            try
            {
                // Get hashtag for given tweitter tweet id
                return await _twitterDbContext.HashTagsEntities.FirstOrDefaultAsync(hastag => hastag.TweeterTweetId == twitterTweetId);
            }
            catch (Exception exception)
            {
                _logger.LogWithParameters(LogLevel.Error, exception, "Unable to complete method due to an exception", parameters);
                throw;
            }
        }

        public Task<List<string>> GetHashTags(int topNthvalue)
        {
            int batchNumber = 0;

            HashTagCollection = new List<string>();
            do
            {
                var hashTags =  _twitterDbContext.HashTagsEntities.OrderByDescending
                                   (hashtag => hashtag.Id).OrderByDescending(hashtag => hashtag.Created)
                                   .Skip(batchNumber * topNthvalue).Take(topNthvalue).ToList();

                

                foreach (var tag in hashTags)
                {
                    if (HashTagCollection.Count == topNthvalue)
                        return Task.FromResult(HashTagCollection);

                    if (!HashTagCollection.Contains(tag.HashTagName) && HashTagCollection.Count < topNthvalue)
                    {
                        HashTagCollection.Add(tag.HashTagName);
                    }
                }

                if (HashTagCollection.Count == topNthvalue)
                    return Task.FromResult(HashTagCollection);

                if (HashTagCollection.Count < topNthvalue)
                {
                    batchNumber = batchNumber + 1;
                }
            } while (HashTagCollection.Count <= topNthvalue);

            return Task.FromResult(HashTagCollection);
        }

        public async Task<List<HashTag>> GetHashTagsPagination(PaginationParams param)
        {
            var hashTags = new List<HashTag>();

            if (param.PageSize >0)
            {
                if (param.PageSize > 100)
                {
                    param.PageSize = 100;
                }
            
             hashTags = await _twitterDbContext.HashTagsEntities.OrderByDescending(hashtag => hashtag.Id)
                    .OrderByDescending(hashtag => hashtag.Created).Skip(param.PageSize * (param.PageNumber-1)).Take(param.PageSize).ToListAsync();
            }

            return hashTags;
        }
    }
}

