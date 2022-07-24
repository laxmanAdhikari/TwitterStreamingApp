using Microsoft.Extensions.Logging;
using Twitter.Core.Constants;
using Twitter.Core.Exceptions;
using Twitter.Core.Extentions;

namespace Twitter.Core.Services
{
    public class TwitterApiTweetService : ITwitterApiTweetService
    {
        protected readonly ILogger<TwitterApiTweetService> _logger;
        public TwitterApiTweetService(ILogger<TwitterApiTweetService> logger)
        {
            _logger = logger;
        }

        public async Task<HttpResponseMessage> GetTweetSearchStreamResponseAsync()
        {
            DotNetEnv.Env.TraversePath().Load();
            var url = string.Format("{0}?{1}", Environment.GetEnvironmentVariable(TwitterConstants.SAMPLE_STREAM_URL), TwitterConstants.EXPANSIONS_AND_FIELD_QUERY);
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

        public async Task GetTweetsSearchStreamAsync(HttpResponseMessage response, Action<string, ILogger, Dictionary<string, object>> onStreamResponse)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Method", "GetTweetsSearchStreamAsync");
            try
            {

                using (var reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();


                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            onStreamResponse?.Invoke(line, _logger, parameters);
                        }
                    }
                }
            }
            catch (ApiException exception)
            {

                _logger.LogWithParameters(LogLevel.Error, exception, exception.Message, parameters);
                throw;
            }
            catch (Exception exception)
            {

                _logger.LogWithParameters(LogLevel.Error, exception, exception.Message, parameters);
                throw;
            }
            finally
            {
                if (response != null)
                {
                    response.Dispose();
                }
            }
        }
    }
}



