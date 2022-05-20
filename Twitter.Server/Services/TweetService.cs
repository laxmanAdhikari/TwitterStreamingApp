using Microsoft.Extensions.Logging;
using Tweetinvi;
using Tweetinvi.Streaming.V2;
using Twitter.Core.Constants;
using Twitter.Core.Exceptions;
using Twitter.Core.Extentions;

namespace Twitter.BlazorServer.Service
{
    public class TweetService : ITweetService
    {
        protected readonly ILogger<TweetService> _logger;
        public TweetService(ILogger<TweetService> logger)
        {
            _logger = logger;
        }

        public async Task<ISampleStreamV2> GetSampleStreamV2()
        {
            string CONSUMER_KEY = "5OVESwZadiXYln3CWPpfii5o3";
            string CONSUMER_SECRET = " YfMz3Kz5fhRu92kWFBTJLHdWEtYdVIZbaaKzkUTcLineAajnwQ";
            string ACCESS_TOKEN = "1525411530875052038-cWPN5dPyNc4n13CKqNklx3Q8FcPqH3";
            string ACCESS_TOKEN_SECRET = " vZQOdbkV3Z2LCVX1bIzsncC06oytYvAvMojj5g4217sCr";
            var userClient = new TwitterClient(CONSUMER_KEY, CONSUMER_SECRET, ACCESS_TOKEN, ACCESS_TOKEN_SECRET);
            var sampleStreamV2 = userClient.StreamsV2.CreateSampleStream();
            return sampleStreamV2;
        }

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
