using Twitter.Model.Entities;
using TwitterStreamApi.Services.Base;

namespace TwitterStreamApi.Services
{
    public interface IAuthorService : IBaseService<Author>
    {
        Task<Author> GetByTwitterAuthorAsync(string twitterAuthorId);

    }
}
