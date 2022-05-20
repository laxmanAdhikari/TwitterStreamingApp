using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Twitter.BlazorServer.Service;
using Twitter.Core.Constants;
using Twitter.Core.Exceptions;
using Twitter.Core.Extentions;

namespace Twitter.BlazorServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TweetStreamController : ControllerBase
    {
        protected readonly ILogger<TweetStreamController> _logger;
        public TweetStreamController(ILogger<TweetStreamController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetTweetSearchStreamResponseAsync()
        {
            var url = string.Format("{0}?{1}", TwitterConstants.SAMPLE_STREAM_URL, TwitterConstants.EXPANSIONS_AND_FIELD_QUERY);
            var parameters = new Dictionary<string, object>();
            parameters.Add("Method", "GetTweetsSearchStreamResponseAsync");
            parameters.Add("Uri", url);

            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                    {
                        DotNetEnv.Env.TraversePath().Load();
                        request.Headers.Add("Authorization", string.Format("Bearer {0}", Environment.GetEnvironmentVariable(TwitterConstants.BEARER_TOKEN)));
                        return await HttpClientAsyncExtensions.GetTwitterApiResponseAsync(httpClient, request);
                    }
                }
            }
            catch (TwitterException exception)
            {
                _logger.LogWithParameters(LogLevel.Error, exception, exception.Message, parameters);
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogWithParameters(LogLevel.Error, exception, exception.Message, parameters);
                throw;
            }
        }
    }
}
