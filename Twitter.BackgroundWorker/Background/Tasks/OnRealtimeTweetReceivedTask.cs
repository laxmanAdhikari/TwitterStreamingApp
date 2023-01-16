using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.SignalR;
using TaskExtensions = TwitterStreamApiHelper.Tasks.Extensions.TaskExtensions;
using TwitterStreamBackgroundTask.Tasks.BaseObject;
using Twitter.Model;
using Twitter.Core.Exceptions;
using Twitter.Core.Extentions;
using Twitter.BackgroundWorker.Hubs.Extensions;
using Twitter.Model.Results;
using Twitter.Service.Services;
using Twitter.BackgroundWorker.Hubs;
using Twitter.Data;

namespace Twitter.BackgroundWorker.Tasks
{

    public class OnRealtimeTweetReceivedTask : BaseTask<OnRealtimeTweetReceivedTask>
    {
        protected readonly string _tweetResponse;
        protected readonly IHubContext<TwitterStreamHub> _twitterHub;
       

        public OnRealtimeTweetReceivedTask([NotNull] IServiceProvider serviceProvider, [NotNull] CancellationToken cancellationToken, string tweetResponse) : base(serviceProvider, cancellationToken)
        {
            _tweetResponse = tweetResponse;

            _twitterHub = serviceProvider.GetService<IHubContext<TwitterStreamHub>>();
        }

        protected override async Task TaskActionAsync(Guid? id)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Method", "TaskActionAsync");
            if (id.HasValue)
            {
                parameters.Add("Job ID", id);
            }

            SearchStreamResult searchStreamResult;
            try
            {
                searchStreamResult = JsonConvert.DeserializeObject<SearchStreamResult>(_tweetResponse);
            }
            catch (Exception exception)
            {
                // Throws exception, if it's unable to convert it into a SearchStreamResult object.
                throw new ApiException("Unable to convert JSON to type SearchStreamResult.", exception, JObject.Parse(_tweetResponse));
            }

            if (searchStreamResult != null && searchStreamResult.Tweet == null)
            {
                // No tweet, so throw an exception.
                throw new ApiException("Tweet does not have an instance of Tweet.", JObject.Parse(_tweetResponse));
            }

            if (searchStreamResult != null && searchStreamResult.Tweet != null && (searchStreamResult.Tweet.ReferenceTweets == null || searchStreamResult.Tweet.ReferenceTweets.Count == 0))
            {
                var tweetParameters = new Dictionary<string, object>();
                foreach (var param in parameters)
                {
                    tweetParameters.Add(param.Key, param.Value);
                }
                tweetParameters.Add("Twitter API Tweet Id", searchStreamResult.Tweet.Id);

                _logger.LogWithParameters(LogLevel.Information, "Start importing incoming realtime tweet", tweetParameters);

                Twitter.Model.Entities.Tweet tweet = null;
                
                // Create a new scope from the service provider.
                using (var scope = _serviceProvider.CreateScope())
                {
                    // Gets the services freom the scope.
                    var twitterDbContext = scope.ServiceProvider.GetService<TwitterDbContext>();
                    var tweetService = scope.ServiceProvider.GetService<ITweetService>();
                    var authorService = scope.ServiceProvider.GetService<IAuthorService>();


                    using (var dbContextTransaction = await twitterDbContext.Database.BeginTransactionAsync())
                    {

                        tweet = await TaskExtensions.CreateUpdateTweetAsync(scope, _logger, searchStreamResult.Tweet, searchStreamResult.Includes.Users, searchStreamResult.Includes.Media);

                        if (tweet == null)
                        {
                            return;
                        }

                        // Commit the transaction to the database.
                        await dbContextTransaction.CommitAsync();
                    }

                    // Send the fact that a tweet has been created to the hub's connected clinets through SignalR.
                    await _twitterHub.CreateTweetAsync(new CreateTweetResults { Data = new List<CreateTweetData> { new CreateTweetData(tweet) } }, _cancellationToken);

                }

                _logger.LogWithParameters(LogLevel.Information, "Finish importing incoming realtime tweet", tweetParameters);
            }
        }

    }
}
