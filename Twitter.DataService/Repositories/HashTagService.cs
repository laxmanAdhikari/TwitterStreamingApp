using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Twitter.Core.Extentions;
using Twitter.Data;
using Twitter.Model;
using Twitter.Model.Entities;
using Twitter.Service.Dto;
using Twitter.Service.Pagination;
using Twitter.Service.Services.Base;

namespace Twitter.Service.Services
{
    public class HashTagService : BaseService<HashTag>, IHashTagService
    {
        private readonly IMapper _mapper;
        protected new readonly DbContextOptions<TwitterDbContext> _twitterDbContext;

        public HashTagService(DbContextOptions<TwitterDbContext> twitterDbContext, ILogger<HashTagService> logger, IMapper mapper) : base(twitterDbContext, logger)
        {

             _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _twitterDbContext = twitterDbContext;
            _mapper = mapper;
        }

        public List<string> HashTagCollection { get; set; }

        public async Task<HashTag> GetByTwitterHashTagAsync(string twitterTweetId)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Method", "GetByTwitterHashTagAsync");
            parameters.Add("Twitter API Tweet Id", twitterTweetId);

            try
            {
                // Get hashtag for given tweitter tweet id
                using var db = new TwitterDbContext(_twitterDbContext);
                return await db.HashTagsEntities.FirstOrDefaultAsync(hastag => hastag.TweeterTweetId == twitterTweetId);
            }
            catch (Exception exception)
            {
                _logger.LogWithParameters(LogLevel.Error, exception, "Unable to complete method due to an exception", parameters);
                throw;
            }
        }

        public async Task<List<string>> GetHashTags(int topNthvalue)
        {
            int batchNumber = 0;

            HashTagCollection = new List<string>();
            do
            {
                using var db = new TwitterDbContext(_twitterDbContext);
                var hashTags = await db.HashTagsEntities.OrderByDescending
                                   (hashtag => hashtag.Id).OrderByDescending(hashtag => hashtag.Created)
                                   .Skip(batchNumber * topNthvalue).Take(topNthvalue).ToListAsync();



                foreach (var tag in hashTags)
                {
                    if (HashTagCollection.Count == topNthvalue)
                        return HashTagCollection;

                    if (!HashTagCollection.Contains(tag.HashTagName) && HashTagCollection.Count < topNthvalue)
                    {
                        HashTagCollection.Add(tag.HashTagName);
                    }
                }

                if (HashTagCollection.Count == topNthvalue)
                    return HashTagCollection;

                if (HashTagCollection.Count < topNthvalue)
                {
                    batchNumber = batchNumber + 1;
                }
            } while (HashTagCollection.Count <= topNthvalue);

            return HashTagCollection;
        }

        public async Task<List<HashTag>> GetHashTags(PaginationParams? param)
        {

            List<HashTag> hashTags = new List<HashTag>();
            using var db = new TwitterDbContext(_twitterDbContext);

            if (param?.PageSize > 0)
            {
                if (param?.PageSize > 100)
                {
                    param.PageSize = 100;
                }

               
                return await db.HashTagsEntities.OrderByDescending(hashtag => hashtag.Id)
                   .OrderByDescending(hashtag => hashtag.Created).Skip(param.PageSize * (param.PageNumber - 1)).Take(param.PageSize).ToListAsync();
            }

            return await db.HashTagsEntities.OrderByDescending(hashtag => hashtag.Id)
                   .OrderByDescending(hashtag => hashtag.Created).ToListAsync();
        }

        public async Task<List<HashTagDto>> GetHashTagsForUI()
        {

            try
            {
                using var db = new TwitterDbContext(_twitterDbContext);

                var hashTags= await db.HashTagsEntities.OrderByDescending(hashtag => hashtag.Id)
                   .OrderByDescending(hashtag => hashtag.Created).ToListAsync().ConfigureAwait(false);

                var hashTagDtos = _mapper.Map<IEnumerable<HashTagDto>>(hashTags).ToList();

                 return hashTagDtos;
            }
            catch(Exception ex) {

                throw ex;
            }
        }
    }
}

