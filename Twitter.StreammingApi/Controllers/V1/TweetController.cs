using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using TwitterStreamApi.Data;
using Twitter.Model;
using TwitterStreamApi.Dto;
using TwitterStreamApi.Background;
using Twitter.Core.Services;
using TwitterStreamApi.Services;
using System.Diagnostics.CodeAnalysis;
using Swashbuckle.AspNetCore.Annotations;

namespace Twitter.StreamApi.Controllers.V1
{
    [ApiController]
    public sealed class TweetController : ControllerBase
    {
        private readonly ITweetService _tweetService;
        private readonly ILogger<TweetController> _logger;
        public TweetController([NotNull] ILogger<TweetController> logger, [NotNull] ITweetService tweetService)
        {

            _tweetService = tweetService;
            _logger = logger;
        }


        [HttpGet]
        [Route("api/v1/count")]
        [SwaggerOperation(Summary = "Get tweet counts.", Description = "Get incremental count of the recent tweets from twitter streaming API.")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTweetCountAsync()
        {
            var parameters = new Dictionary<string, object>();

            parameters.Add("Method", "GetTweetCountAsync");

            try
            {
                return Ok(await _tweetService.GetCount());
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }

        }
    }
}
