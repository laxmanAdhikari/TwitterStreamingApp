using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.BackgroundWorker
{
    public class BackgroundJob
    {

        public Guid Id { get; private set; }

        public DateTimeOffset Created { get; }

        public Func<Guid, Task> JobActionAsync { get; }

        public bool Async { get; }

        public DateTimeOffset? Completed { get; private set; }

        public BackgroundJob(Func<Guid, Task> jobActionAsync)
        {
            Id = Guid.NewGuid();
            Created = DateTimeOffset.Now;
            JobActionAsync = jobActionAsync;
        }


        public BackgroundJob(Func<Guid, Task> jobActionAsync, bool async ) : this(jobActionAsync)
        {
            Async = async;
        }

        public async Task RunJobAsync()
        {
            try
            {
                await JobActionAsync?.Invoke(Id);
            }
            catch(Exception)
            {

            }
        }

        public void MarkAsComplete()
        {
            Completed = DateTimeOffset.Now;
        }
    }
}
