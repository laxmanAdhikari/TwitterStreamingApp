using System.Diagnostics.CodeAnalysis;
using Twitter.Core.Extentions;

namespace TwitterStreamApi.Background
{
    public class BackgroundJobService : IBackgroundJobService
    {
        private SemaphoreSlim JobRunningLock;

        protected readonly ILogger<BackgroundJobService> _logger;

        protected readonly IHostApplicationLifetime _hostApplicationLifetime;

        public BackgroundJobService([NotNull] ILogger<BackgroundJobService> logger, IHostApplicationLifetime hostApplicationLifetime)
        {

            _logger = logger;
            _hostApplicationLifetime = hostApplicationLifetime;

            JobRunningLock = new SemaphoreSlim(1, 1);
        }
        public  async Task RunJobAsync(BackgroundJob backgroundJob)
        {
            var parameters = new Dictionary<string, object>();

            parameters.Add("Method", "RunJobAsync");

            parameters.Add("Job ID", backgroundJob.Id.ToString());

            if (backgroundJob == null || backgroundJob.JobActionAsync == null || backgroundJob.Completed.HasValue) 
            {
                return; 
            }

            try
            {
                _logger.LogWithParameters(LogLevel.Debug, "Wait for all other jobs to complete.", parameters);
                await JobRunningLock.WaitAsync(_hostApplicationLifetime.ApplicationStopping);

                _logger.LogWithParameters(LogLevel.Information , "Start running job.", parameters);

            }
            catch(Exception exception)
            {
                _logger.LogWithParameters(LogLevel.Error, exception, exception.Message, parameters);
            }
            finally
            {
                backgroundJob.MarkAsComplete();
                _logger.LogWithParameters(LogLevel.Information, "Finishing running job.", parameters);

                JobRunningLock.Release();

            }

        }
    }
}