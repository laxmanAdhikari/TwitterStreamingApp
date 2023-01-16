using System.Diagnostics.CodeAnalysis;
using Twitter.Core.Exceptions;
using Twitter.Core.Extentions;

namespace Twitter.BackgroundWorker.BaseObject
{
    public abstract class BaseTask<TTask>
    {

        protected readonly IServiceProvider _serviceProvider;

        protected readonly CancellationToken _cancellationToken;

        protected readonly ILogger<TTask> _logger;

        public bool TaskRunning { get; private set; }

        protected abstract Task TaskActionAsync(Guid? id);


        protected BaseTask([NotNull] IServiceProvider serviceProvider, [NotNull] CancellationToken cancellationToken)
        {
            _serviceProvider = serviceProvider;
            _cancellationToken = cancellationToken;

            using (var scope = serviceProvider.CreateScope())
            {
                _logger=  scope.ServiceProvider.GetService<ILogger<TTask>>();
            }
        }


        public async Task RunAsync(Guid? id)
        {

            await RunTaskAsync(id);
        }

        private async Task RunTaskAsync(Guid? id)
        {
            TaskRunning = true;

            var parameters = new Dictionary<string, object>();

            parameters.Add("Method", "RunTasjAsync");

            if( id !=null)
            {
                parameters.Add("Job ID", id);
            }

            try
            {
                await TaskActionAsync(id);
            }
            catch (TwitterException exception)
            {
                _logger.LogWithParameters(LogLevel.Error, exception, "Unable to complete due to an exception", parameters);
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogWithParameters(LogLevel.Error, exception, "Unable to complete method due to an exception", parameters);
                throw;
            }
            finally
            {
                TaskRunning = false;
            }

        }
    }
}
