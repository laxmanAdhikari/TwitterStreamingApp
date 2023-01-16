using Twitter.Model.Entities;
using Twitter.Service.Services.Base;

namespace Twitter.Service.Services
{
    public interface IAuthorService : IBaseService<Author>
    {
        Task<Author> GetByTwitterAuthorAsync(string twitterAuthorId);

    }
}
