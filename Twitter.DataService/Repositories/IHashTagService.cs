using Twitter.Model.Entities;
using Twitter.Service.Dto;
using Twitter.Service.Pagination;
using Twitter.Service.Services.Base;

namespace Twitter.Service.Services
{
    public interface IHashTagService : IBaseService<HashTag>
    {
        Task <HashTag> GetByTwitterHashTagAsync(string twitterTweetId);
        Task<List<string>> GetHashTags(int topNthvalue);
        Task<List<HashTag>> GetHashTags(PaginationParams? @params);
        Task<List<HashTagDto>> GetHashTagsForUI();

    }
}
