using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Streaming.V2;

namespace Twitter.BlazorServer.Service
{
    public  interface ITweetService
    {
        Task<HttpResponseMessage> GetTweetSearchStreamResponseAsync();
        Task GetTweetsSearchStreamAsync(HttpResponseMessage response, Action<string, ILogger, Dictionary<string, object>> onSteeamResponse);

        Task<ISampleStreamV2> GetSampleStreamV2();
    }
}
