using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics.CodeAnalysis;
using Twitter.Core.Exceptions;
using Twitter.Service.Pagination;
using Twitter.Service.Services;

namespace Twitter.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    public class HashTagController : ControllerBase
    {
        private readonly IHashTagService _hashTagService;
        private readonly ILogger<HashTagController> _logger;
        public HashTagController([NotNull] ILogger<HashTagController> logger, [NotNull] IHashTagService hashTagService)
        {

            _hashTagService = hashTagService;
            _logger = logger;
        }

        [HttpGet, MapToApiVersion("1.0")]
        [Route("api/v{version:apiVersion}/hashtags/{topnth}")]
        [SwaggerOperation(Summary = "Get HashTags", Description = "Get top nth HashTags from the recent tweets.")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTopNthHashTags(int topnth)
        {
            var parameters = new Dictionary<string, object>
            {
                { "Method", "GetTopNHashTagsAsync" }
            };

            _logger.LogInformation("Get top Nth Hash Tags");

            try
            {
                var result = await _hashTagService.GetHashTags(topnth);

                return Ok(result);
            }
            catch (Exception exception)

            {
                _logger.LogError(exception,$"error{exception.Message}", parameters.ToArray());
                return BadRequest(exception.Message);
            }

        }

        [HttpGet, MapToApiVersion("1.0")]
        [Route("api/v{version:apiVersion}/hashtags/pagination")]
        [SwaggerOperation(Summary = "Get HashTags", Description = "Get Paginated Hashtags from the recent tweets.")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPaginationResults([FromQuery] PaginationParams param)
        {

            try
            {
                var hashTags = await _hashTagService.GetHashTags(param);
                List<string> HashTagCollection = new();

                foreach (var tag in hashTags.ToList())
                {
                    
                    if (!HashTagCollection.Contains(tag.HashTagName))
                    {
                        HashTagCollection.Add(tag.HashTagName);
                    }
                }

                return Ok(HashTagCollection);
            }
            catch (Exception ex)
            {
                throw new TwitterException($"Error occured {ex}");

            }

        }
    }
}
