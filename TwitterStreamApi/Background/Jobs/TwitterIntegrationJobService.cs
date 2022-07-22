using System.Diagnostics.CodeAnalysis;
using Twitter.Core.Extentions;

namespace TwitterStreamApi.Background.Jobs
{
    public class TwitterIntegrationJobService : ITwitterIntegrationJobService
    {
        private SemaphoreSlim JobRunningLock;
        protected readonly ILogger<TwitterIntegrationJobService> _logger;
        protected readonly IHostApplicationLifetime _hostApplicationLifetime;

        public TwitterIntegrationJobService([NotNull] ILogger<TwitterIntegrationJobService> logger, IHostApplicationLifetime hostApplicationLifetime)
        {
            _logger = logger;
            _hostApplicationLifetime = hostApplicationLifetime;

            JobRunningLock = new SemaphoreSlim(1, 1); // Ensures that only one job is ran at a time.
        }

        public async Task RunJobAsync(TwitterIntegrationJob twitterIntegrationJob)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Method", "RunJobAsync");
            parameters.Add("Job ID", twitterIntegrationJob.Id.ToString());

            if (twitterIntegrationJob == null || twitterIntegrationJob.JobActionAsync == null || twitterIntegrationJob.Completed.HasValue)
            {
                return;
            }

            try
            {
                // Ensure that only one job can be run at a time.
                _logger.LogWithParameters(LogLevel.Debug, "Wait for all other jobs to complete.", parameters);
                await JobRunningLock.WaitAsync(_hostApplicationLifetime.ApplicationStopping); // Only one job can run at one time.
                _logger.LogWithParameters(LogLevel.Information, "Start running job.", parameters);

                // Invoke the job, passing in the job's identifier (for logging purposes).
                await twitterIntegrationJob.JobActionAsync.Invoke(twitterIntegrationJob.Id);
            }
            catch (Exception exception)
            {
                // Log the error it is failes.
                _logger.LogWithParameters(LogLevel.Error, exception, exception.Message, parameters);
            }
            finally
            {               
                twitterIntegrationJob.MarkAsComplete(); // Mark the job as complete.
                _logger.LogWithParameters(LogLevel.Information, "Finish running job.", parameters);
                JobRunningLock.Release(); // Release the lock so other jobs can run.
            }
        }
    }
}
