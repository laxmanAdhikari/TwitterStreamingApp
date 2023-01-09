using Microsoft.AspNetCore.Mvc;
using Moq;
using Sprache;
using System.Configuration;
using System.Net;
using Twitter.Service.Services;
using Twitter.StreamApi.Controllers.V1;
using Twitter.StreammingApi.Controllers.V1;

namespace Twitter.StreammimgApi.Tests
{
    [TestFixture]
    public class HashTagApiTests
    {

        private const string REQUEST_URL = "api/v1/hashtags";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase(10)]
        public async Task HashTagController_GetTopNthHashTags_Call_Successful(int topNth)
        {

            await using var testServer = new ApiTestsServer();

            using var client = testServer.CreateJsonClient();

            var apiUrl = $"{REQUEST_URL}/{topNth}";
            using HttpResponseMessage response = client.GetAsync(apiUrl).Result;

            Assert.IsNotNull(response);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));


        }

        [Test]
        [TestCase(10)]
        public async Task HashTagController_GetTopNthHashTags_Success(int topNth)
        {
            // Arrange
            List<string> topNthHashtageResponse = new List<string>()
            {
            "one",
            "two",
            "three",
            "four",
            "five",
            "six",
            "seven",
            "eight",
            "nine",
            "ten"
            };

            var mockTweetService = new Mock<IHashTagService>();
            mockTweetService.Setup(x => x.GetHashTags(topNth))
                .Returns(Task.FromResult(topNthHashtageResponse));

            var controller = new HashTagController(null, mockTweetService.Object);


            // Act
            var response = await controller.GetTopNthHashTags(topNth);

            var okResult = response as OkObjectResult;
            var actualResult = okResult.Value as List<string>;


            // Assert
            Assert.IsNotNull(response);
            Assert.That(actualResult.Count, Is.EqualTo(topNth));
        }

        [Test]
        [TestCase(10)]
        public void HashTagController_GetTopNthHashTags_ThrowException(int topNthHashTag)
        {
            // Arrange
            var mockTweetService = new Mock<IHashTagService>();
            mockTweetService.Setup(x => x.GetHashTags(topNthHashTag))
                .Throws(new Exception("Error occured while gettign the hastags."));
               
            var controller = new HashTagController(null, mockTweetService.Object);

            // Act
            var response = controller.GetTopNthHashTags(topNthHashTag).Result;
            var errorMessage = response.GetType().GetProperty("Value").GetValue(response);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(errorMessage);
            Assert.That(errorMessage, Is.EqualTo("Error occured while gettign the hastags."));
        }

    }
}