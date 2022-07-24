using System.Net;
using System.Text.Json;
using Twitter.Core.Exceptions;

namespace TwitterStreamApi.Pipeline
{
    public class ErrorHandling
    {
        private readonly RequestDelegate _next;

        public ErrorHandling(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                var responseModel = Response<string>.Fail(error.Message);

                switch (error)
                {
                    case NotImplementedException:
                        response.StatusCode = (int)HttpStatusCode.NotImplemented;
                        break;
                    case ApiException:
                    case TwitterException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);

            }
        }
    }
}


