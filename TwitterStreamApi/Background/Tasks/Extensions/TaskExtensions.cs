using Twitter.Core.Exceptions;
using Twitter.Model;
using Twitter.Model.Entities;
using TwitterStreamApi.Services;

namespace TwitterStreamApi.Tasks.Extensions
{
    public static class TaskExtensions
    {
        public static async Task<Twitter.Model.Entities.Tweet> CreateUpdateTweetAsync(IServiceScope scope, ILogger logger, Twitter.Model.Tweet apiTweet, List<User> users, List<Media> media)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Static Class", "TaskExtensions");
            parameters.Add("Method", "CreateUpdateTweetAsync");
            parameters.Add("Twitter API Tweet Id", apiTweet.Id);

            try
            {
                // Get the services from the scope.
                var authorService = scope.ServiceProvider.GetService<IAuthorService>();
                var tweetService = scope.ServiceProvider.GetService<ITweetService>();

                if (apiTweet.ReferenceTweets != null && apiTweet.ReferenceTweets.Count > 0)
                {
                    // Not interested in referenced tweets.
                    return null;
                }

                var author = await authorService.GetByTwitterAuthorAsync(apiTweet.AuthorId);

                var authorUser = users.FirstOrDefault(user => user.Id == apiTweet.AuthorId);

                if (authorUser == null)
                {
                    throw new ApiException(string.Format("Cannot find the author that references '{0}'", author.Id));
                }

                var authorCreate = author == null;

                if (authorCreate)
                {
                    author = new Author();
                }

                author.TwwitterAuthorId = authorUser.Id;
                author.TwitterName = authorUser.Name;
                author.TwitterHandle = authorUser.UserName;
                author.TwitterImageUrl = authorUser.ProfileImageUrl;

                if (authorCreate)
                {

                    await authorService.CreateAsync(author);
                }
                else
                {
                    await authorService.UpdateAsync(author.Id, author);
                }


                var tweet = await tweetService.GetByTwitterTweetAsync(apiTweet.Id);

                var tweetCreate = tweet == null;

                if (tweetCreate)
                {
                    tweet = new Twitter.Model.Entities.Tweet();
                }

                // Update tweet properties.

                tweet.AuthorId = author.Id;
                tweet.TweeterTweetId = apiTweet.Id;
                tweet.Content =  apiTweet.Text;

                Console.WriteLine(apiTweet.Text);

                tweet.TwitterPublished = apiTweet.Created;

                if (tweetCreate)
                {
                    await tweetService.CreateAsync(tweet);
                }
                else
                {
                    await tweetService.UpdateAsync(tweet.Id, tweet);
                }

                logger.Log(LogLevel.Debug, string.Format("Tweet has been imported to the Tweet table in the database (id: '{0}')", tweet.Id), parameters);

                return tweet;
            }
            catch (ApiException exception)
            {
                logger.Log(LogLevel.Error, exception, exception.Message, parameters);
                throw;
            }
            catch (Exception exception)
            {
                logger.Log(LogLevel.Error, exception, exception.Message, parameters);
                throw;
            }
        }

    }
}

