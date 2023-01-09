using Twitter.Model.Entities;
using Twitter.Service.Services.Base;

namespace Twitter.Service.Services
{
    public interface ITweetService : IBaseService<Tweet>
    {   
        Task<Tweet> GetByTwitterTweetAsync(string twitterTweetId);

        Task<int> GetCount();

    }
}
