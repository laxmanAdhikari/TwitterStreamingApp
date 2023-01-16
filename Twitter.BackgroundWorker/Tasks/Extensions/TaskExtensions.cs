using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Twitter.Core.Exceptions;
using Twitter.Model;
using Twitter.Model.Entities;
using Twitter.Service.Services;
using Tweet = Twitter.Model.Entities.Tweet;

namespace TwitterStreamApiHelper.Tasks.Extensions
{
    public static class TaskExtensions
    {
        public static async Task<Tweet> CreateUpdateTweetAsync(IServiceScope scope, ILogger logger, Twitter.Model.Tweet apiTweet, List<User> users, List<Media> media)
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
                var hashTagService= scope.ServiceProvider.GetService<IHashTagService>();

                if (apiTweet.ReferenceTweets != null && apiTweet.ReferenceTweets.Count > 0)
                {
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

                // Update author properties.
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
                tweet.Content = ConvertMediaAndLinks(apiTweet, media);
                tweet.TwitterPublished = apiTweet.Created;

                if (tweetCreate)
                {
                    await tweetService.CreateAsync(tweet);
                }
                else
                {
                    await tweetService.UpdateAsync(tweet.Id, tweet);
                }

                var hashTag = await hashTagService.GetByTwitterHashTagAsync(apiTweet.Id);

                var hasTagCreate = hashTag == null;

                if (hasTagCreate)
                {
                    hashTag = new Twitter.Model.Entities.HashTag();
                }

                
                hashTag.TweeterTweetId = apiTweet.Id;
                hashTag.AuthorId = apiTweet.AuthorId;
                hashTag.HashTagName = hashTag.HashTagName;

                if (hasTagCreate)
                {
                    if (apiTweet.Entities.hashtags != null)
                    {
                        foreach (var item in apiTweet.Entities.hashtags)
                        {
                            hashTag.TweeterTweetId = apiTweet.Id;
                            hashTag.AuthorId = author.Id.ToString();
                            hashTag.HashTagName = item.tag;
                        }
                        await hashTagService.CreateAsync(hashTag);
                    }
                }
                else
                {
                    await hashTagService.UpdateAsync(hashTag.Id, hashTag);
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

        
        private static string ConvertMediaAndLinks(Twitter.Model.Tweet twitterApiTweet, List<Media> twitterApiMedia)
        {
            if (twitterApiTweet == null || string.IsNullOrWhiteSpace(twitterApiTweet.Text))
            {
                return null;
            }

            if (twitterApiTweet.Attachments != null && twitterApiTweet.Attachments.MediaKeys != null && twitterApiTweet.Attachments.MediaKeys.Count() > 0 && twitterApiMedia != null && twitterApiMedia.Count() > 0)
            {
                var images = string.Empty;
                foreach (var mediaKey in twitterApiTweet.Attachments.MediaKeys)
                {
                    var media = twitterApiMedia.FirstOrDefault(media => media.MediaKey == mediaKey);

                    if (media == null || media.Type != "photo")
                    {
                        continue;
                    }

                    images += string.Format("\r\n<img src=\"{0}\" alt=\"{1}\" style=\"max-height: 200px; max-width:100%; height: auto\" />", media.Url, media.MediaKey);
                }

                if (!string.IsNullOrWhiteSpace(images))
                {
                    var imageRegex = new Regex(@"( )?https:\/\/t\.co\/([a-z0-9A-Z]+)$", RegexOptions.Singleline);

                    if (imageRegex.IsMatch(twitterApiTweet.Text))
                    {
                        twitterApiTweet.Text = imageRegex.Replace(twitterApiTweet.Text, "");
                    }

                    twitterApiTweet.Text += images;
                }
            }

            var linkRegex = new Regex(@"(https:\/\/t\.co\/([a-z0-9A-Z]+))");

            if (linkRegex.IsMatch(twitterApiTweet.Text))
            {
                twitterApiTweet.Text = linkRegex.Replace(twitterApiTweet.Text, "<a href=\"$1\">$1</a>");
            }

            return twitterApiTweet.Text;
        }

    }
}

