using Twitter.Model.Entities;
using Twitter.Service.Pagination;
using TwitterStreamApi.Services.Base;

namespace TwitterStreamApi.Services
{
    public interface IHashTagService : IBaseService<HashTag>
    {
        Task<HashTag> GetByTwitterHashTagAsync(string twitterTweetId);

        Task<List<string>> GetHashTags(int topNthvalue);

        Task<List<HashTag>> GetHashTags(PaginationParams @params = null);


    }
}
