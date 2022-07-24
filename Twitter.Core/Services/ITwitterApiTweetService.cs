using Microsoft.Extensions.Logging;

namespace Twitter.Core.Services
{
    public interface ITwitterApiTweetService
    {
        Task<HttpResponseMessage> GetTweetSearchStreamResponseAsync();

        Task GetTweetsSearchStreamAsync(HttpResponseMessage response, Action<string, ILogger, Dictionary<string, object>> onStreamResponse);
    }

}
