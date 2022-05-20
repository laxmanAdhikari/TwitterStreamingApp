using Newtonsoft.Json.Linq;
using Twitter.Core.Exceptions;

namespace Twitter.Core.Extentions
{
    public static class HttpClientAsyncExtensions
    {
        public static async Task<HttpResponseMessage> GetTwitterApiResponseAsync(HttpClient httpClient, HttpRequestMessage request)
        {
            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            if (response == null)
            {
                throw new TwitterException("Unable to get a response from the Twitter API.");
            }

            if (!((int)response.StatusCode).ToString().StartsWith("2"))
            {
                var content = await response.Content.ReadAsStringAsync();

                JObject jsonErrorContent = null;
                var errorMessages = string.Empty;

                try
                {
                    jsonErrorContent = JObject.Parse(content);
                }
                catch (Exception)
                {
                    errorMessages += content;
                }

                if (jsonErrorContent != null) {
                    JToken errorJson = jsonErrorContent["errors"];

                    if (errorJson != null && errorJson.Type == JTokenType.Array)
                    {
                        if (errorJson[0]["message"] == null || errorJson[0]["message"].Type == JTokenType.Null)
                        {
                            errorJson = errorJson["errors"];
                        }

                        if (errorJson != null && errorJson.Type == JTokenType.Array)
                        {
                            foreach (var error in errorJson)
                            {
                                if (error["message"] != null && error["message"].Type != JTokenType.Null)
                                {
                                    errorMessages += (!string.IsNullOrWhiteSpace(errorMessages) ? "\r\n\r\n" : "") + error["message"];
                                }
                            }
                        }
                    }
                    else if (jsonErrorContent["detail"] != null && jsonErrorContent["detail"].Type != JTokenType.Null)
                    {
                        errorMessages += (!string.IsNullOrWhiteSpace(errorMessages) ? "\r\n\r\n" : "") + jsonErrorContent["detail"];
                    }

                }

                int? xRateLimitReset = null;
                int xRate;
                if (response.Headers != null && response.Headers.Contains("x-rate-limit-reset") && int.TryParse(response.Headers.GetValues("x-rate-limit-reset").FirstOrDefault(), out xRate))
                {
                    xRateLimitReset = xRate;
                }

                throw new TwitterException(string.Format("The API returned a '{0}' response.{1}", (int)response.StatusCode, !string.IsNullOrWhiteSpace(errorMessages) ? "The following messages returned:\r\n" + errorMessages + "\r\n" : ""), xRateLimitReset);
            }

            return response;
        }
    }
}
