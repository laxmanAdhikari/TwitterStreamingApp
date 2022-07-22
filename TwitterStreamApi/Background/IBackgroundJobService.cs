using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterStreamApi.Background
{
    public interface IBackgroundJobService
    {
        Task RunJobAsync(BackgroundJob backgroundJob);
    }
}
