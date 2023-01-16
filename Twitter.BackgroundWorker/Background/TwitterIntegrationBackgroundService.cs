using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.SignalR;
using Twitter.Core.Extentions;
using Twitter.BackgroundWorker.Configuration;
using Twitter.BackgroundWorker.Hubs;
using Twitter.BackgroundWorker.Jobs;
using Twitter.BackgroundWorker.Tasks;

namespace Twitter.BackgroundWorker.Background
{
    public class TwitterIntegrationBackgroundService : IHostedService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ILogger<TwitterIntegrationBackgroundService> _logger;
        protected readonly IOptions<ApiConfiguration> _apiConfiguration;
        protected readonly IHubContext<TwitterStreamHub> _twitterHub;
        protected readonly ITwitterIntegrationJobService _twitterIntegrationJobService;
        protected readonly IHostApplicationLifetime _hostApplicationLifetime;

        protected ListenForRealtimeTweetsFromTwitterApiTask ListenForRealtimeTweetsFromTwitterApiTask { get; set; }
        public TwitterIntegrationBackgroundService([NotNull] IServiceProvider serviceProvider,
            [NotNull] ILogger<TwitterIntegrationBackgroundService> logger, [NotNull] IOptions<ApiConfiguration> apiConfiguration,
            [NotNull] IHubContext<TwitterStreamHub> twitterStreamHub,
            [NotNull] ITwitterIntegrationJobService twitterIntegrationJobService,
            [NotNull] IHostApplicationLifetime hostApplicationLifetime)
        {
            _serviceProvider = serviceProvider;
            _apiConfiguration = apiConfiguration;
            _logger = logger;
            _twitterHub = twitterStreamHub;
            _twitterIntegrationJobService = twitterIntegrationJobService;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Method", "StartAsync");


            _logger.LogWithParameters(LogLevel.Information, "Starting Twitter Integration Background Service...", parameters);

            // Create Tasks
            
            ListenForRealtimeTweetsFromTwitterApiTask = new ListenForRealtimeTweetsFromTwitterApiTask(_serviceProvider, _hostApplicationLifetime.ApplicationStopping);

            // Process Tweet when Recieved
            ListenForRealtimeTweetsFromTwitterApiTask.OnTweetReceived += async (tweetResponse) =>
            {
                // When a tweet is received run the 'OnRealtimeTweetReceivedTask' task.
                await _twitterIntegrationJobService.RunJobAsync(new TwitterIntegrationJob((Guid id) => new OnRealtimeTweetReceivedTask(_serviceProvider, _hostApplicationLifetime.ApplicationStopping, tweetResponse).RunAsync(id)));
            };

            await _twitterIntegrationJobService.RunJobAsync(new TwitterIntegrationJob((Guid id) => ListenForRealtimeTweetsFromTwitterApiTask.RunAsync(id)));

            // Schedule a task to import for the recent tweets at regular intervals.
            SetUpSchedule();

            _logger.LogWithParameters(LogLevel.Information, "Started Twitter Integration Background Service", parameters);

     
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Method", "StopAsync");

            _logger.LogWithParameters(LogLevel.Information, "Stopping Twitter Integration Background Service...", parameters);

            var task = Task.CompletedTask;
            _logger.LogWithParameters(LogLevel.Information, "Stopped Twitter Integration Background Service", parameters);

            return task;
        }

        private void SetUpSchedule()
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Method", "SetUpSchedule");

            // Run async, so we are not holding up the ability to start the hosted service.
            Task.Run(async () =>
            {
                // Continue to run the task until cancellation is requested.
                while (!_hostApplicationLifetime.ApplicationStopping.IsCancellationRequested)
                {
                    var importRecentTweetsSchedule = new TimeSpan(0, 2, 0);
                    var nextSchedule = Task.Delay(importRecentTweetsSchedule, _hostApplicationLifetime.ApplicationStopping); // Delay the schedule until the sync recent schedule has been reached.
                    _logger.LogWithParameters(LogLevel.Information, string.Format("Set up schedule for importing recent tweets. The importing of recent tweets will happen every {0}", importRecentTweetsSchedule.ToString("h\\:mm\\:ss")), parameters);
                    await nextSchedule;
                };
            });
        }
    }
}
