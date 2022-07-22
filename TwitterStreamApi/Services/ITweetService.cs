using Twitter.Model.Entities;
using TwitterStreamApi.Services.Base;


namespace TwitterStreamApi.Services
{
    public interface ITweetService : IBaseService<Tweet>
    {
        
        Task<Tweet> GetByTwitterTweetAsync(string twitterTweetId);


        Task<int> GetCount();

        Task<List<string>> GetHashTags(int topNthvalue);
    }
}
