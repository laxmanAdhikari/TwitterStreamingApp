﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics.CodeAnalysis;

namespace Twitter.StreammingApi.Controllers.V1
{
    [ApiController]
    public class HashTagController : ControllerBase
    {
        private readonly IHashTagService _hashTagService;
        private readonly ILogger<HashTagController> _logger;
        public HashTagController([NotNull] ILogger<HashTagController> logger, [NotNull] IHashTagService hashTagService)
        {

            _hashTagService = hashTagService;
            _logger = logger;
        }

        [HttpGet]
        [Route("api/v1/hashtags/{topnth}")]
        [SwaggerOperation(Summary = "Get HashTags", Description = "Get top nth HashTags from the recent tweets.")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<string>> GetTopNthHashTags(int topnth)
        {
            var parameters = new Dictionary<string, object>();

            parameters.Add("Method", "GetTopNHashTagsAsync");

            try
            {
                var result = _hashTagService.GetHashTags(topnth);

                return Ok(result);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }

        }
    }
}