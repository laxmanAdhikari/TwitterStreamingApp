using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Twitter.StreamApi.Controllers.V1;
using TwitterStreamApi.Services;

namespace Twitter.StreammimgApi.Tests
{
    [TestFixture]
    public class TwitterStreammingApiTests
    {

        private const string REQUEST_URL = "/api/v1/count";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task TweetStreammingController_GetTweetCountAsync_Call_Successful()
        {

            await using var testServer = new ApiTestsServer();

            using var client = testServer.CreateJsonClient();

            using HttpResponseMessage response = client.GetAsync(REQUEST_URL).Result;

            Assert.IsNotNull(response);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));


        }

        [Test]
        public async Task TweetStreammingController_GetTweetCountAsync_Success()
        {
            // Arrange
            var mockTweetService = new Mock<ITweetService>();
            mockTweetService.Setup(x => x.GetCount())
                .Returns(Task.FromResult(1000));
            var controller = new TweetStreammingController(null, mockTweetService.Object);

            // Act
            var response = await controller.GetTweetCountAsync();
            var actualCount = (int)response.GetType().GetProperty("Value").GetValue(response);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(actualCount);
            Assert.That(actualCount, Is.EqualTo(1000));
        }

        [Test]
        public async Task TweetStreammingController_GetTweetCountAsync_ThrowException()
        {
            // Arrange
            var mockTweetService = new Mock<ITweetService>();
            mockTweetService.Setup(x => x.GetCount())
                .Throws(new InvalidOperationException());
               
            var controller = new TweetStreammingController(null, mockTweetService.Object);

            // Act
            var response = await controller.GetTweetCountAsync();
            var errorMessage = response.GetType().GetProperty("Value").GetValue(response);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(errorMessage);
            Assert.That(errorMessage, Is.EqualTo("Operation is not valid due to the current state of the object."));
        }

    }
}