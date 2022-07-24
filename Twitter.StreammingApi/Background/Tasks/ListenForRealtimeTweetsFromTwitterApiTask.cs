using System.Diagnostics.CodeAnalysis;
using Twitter.Core.Exceptions;
using Twitter.Core.Extentions;
using TwitterStreamBackgroundTask.Tasks.BaseObject;

namespace TwitterStreamApi.Background.Tasks
{
    public class ListenForRealtimeTweetsFromTwitterApiTask : BaseTask<ListenForRealtimeTweetsFromTwitterApiTask>
    {
        protected readonly ITwitterApiTweetService _twitterApiTweetService;

        public event Action<string> OnTweetReceived;

        
        public ListenForRealtimeTweetsFromTwitterApiTask([NotNull] IServiceProvider serviceProvider, [NotNull] CancellationToken cancellationToken) : base(serviceProvider, cancellationToken)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                _twitterApiTweetService = scope.ServiceProvider.GetService<ITwitterApiTweetService>();
            }
        }

        protected override async Task TaskActionAsync(Guid? id)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Method", "TaskActionAsync");
            if (id.HasValue)
            {
                parameters.Add("Job ID", id);
            }

            try
            {
                if (!_cancellationToken.IsCancellationRequested)
                {
                    SearchStream(id, await _twitterApiTweetService.GetTweetSearchStreamResponseAsync());
                }
            }
            catch (TwitterException exception)
            {
                ReconnectToStream(id, exception.XRateLimitResetDate);
            }
            catch (Exception)
            {
                ReconnectToStream(id, null);
            }
        }

        protected void SearchStream(Guid? id, HttpResponseMessage response)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Method", "SearchStream");
            if (id.HasValue)
            {
                parameters.Add("Job ID", id);
            }

            Task.Run(async () =>
            {
                try
                {
                    _logger.LogWithParameters(LogLevel.Information, "Start importing realtime tweets from Twitter API", parameters);

                    await _twitterApiTweetService.GetTweetsSearchStreamAsync(response, (realTimeTweet, logger, parameters) =>
                    {
                        if (OnTweetReceived != null)
                        {
                            OnTweetReceived.Invoke(realTimeTweet);
                        }
                    });

                    _logger.LogWithParameters(LogLevel.Warning, "Finish importing realtime tweets from Twitter API", parameters);
                }
                catch (TwitterException exception)
                {
                    _logger.LogWithParameters(LogLevel.Error, exception, "Disconnected from the Twitter API", parameters);
                    ReconnectToStream(id, exception.XRateLimitResetDate);
                }
                catch (Exception exception)
                {
                    _logger.LogWithParameters(LogLevel.Error, exception, "Disconnected from the Twitter API", parameters);
                    ReconnectToStream(id, null);
                }
            }, _cancellationToken);

        }

        protected void ReconnectToStream(Guid? id, DateTimeOffset? reconnectTime)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Method", "ReconnectToStream");
            if (id.HasValue)
            {
                parameters.Add("Job ID", id);
            }

            // Run the task to reconnect async, as there will be a time delay before it attempts to reconnect to the Twitter stream.
            Task.Run(async () =>
            {
                var interval = reconnectTime.HasValue ? reconnectTime.Value.Subtract(DateTimeOffset.Now) : new TimeSpan(0, 1, 0); // If a reconnect time is supplied, delay the reconnection of the stream until the reconnect time has been reached.

                _logger.LogWithParameters(LogLevel.Information, string.Format("Wait {0} before trying again.", interval.ToString("h\\:mm\\:ss")), parameters);
                await Task.Delay(interval, _cancellationToken); // Delay before reconnecting to the stream.
                await RunAsync(id); // Re-run the task.
            }, _cancellationToken);
        }

    }
}

