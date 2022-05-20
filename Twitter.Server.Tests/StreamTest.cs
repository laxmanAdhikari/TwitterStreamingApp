using FakeItEasy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Events;
using Tweetinvi.Models;
using Tweetinvi.Streaming;
using Tweetinvi.Streaming.V2;
using Xunit;
using TweetinviContainer = Tweetinvi.Injectinvi.TweetinviContainer;
namespace Twitter.Server.Tests
{
    public class StreamTest
    {

        private Tuple<ISampleStreamV2, Func<Action<string>>> InitForCatchingJsonEvents()
        {
            // arrange
            var fakeStreamResultGenerator = A.Fake<ISampleStreamV2>();

            var container = new TweetinviContainer();
            container.BeforeRegistrationCompletes += (sender, args) =>
            {
                container.RegisterInstance(typeof(ISampleStreamV2), fakeStreamResultGenerator);
            };
            container.Initialize();

            Action<string> EventReceivedCallback = null;

            A.CallTo(() => fakeStreamResultGenerator.StartAsync())
                .ReturnsLazily(callInfo =>
                {
                    EventReceivedCallback = callInfo.Arguments.Get<Action<string>>("EventReceived");
                    return Task.CompletedTask;
                });

            var client = new TwitterClient(A.Fake<ITwitterCredentials>(), new TwitterClientParameters
            {
                Container = container
            });

            var appCredentials = new ConsumerOnlyCredentials("", "")
            {
                BearerToken = "",
            };

            var appClient = new TwitterClient(appCredentials);

            var fs = appClient.StreamsV2.CreateSampleStream();
            return new Tuple<ISampleStreamV2, Func<Action<string>>>(fs, () => EventReceivedCallback);
        }

        [Fact]
        public async void TweetEventRaised()
        {

            // arrange
            var tuple = InitForCatchingJsonEvents();
            var fs = tuple.Item1;

            // act
            

            var TweetReceived = false;
            fs.TweetReceived += (sender, args) =>
            {
                Assert.NotNull(args.Json);
                TweetReceived = true;
            };

           // await fs.StartAsync();

            var json = File.ReadAllText("./TestData/TweetReceived.json");
            var jsonReceivedCallback = tuple.Item2();
            jsonReceivedCallback(json);

            // assert
            await Task.Delay(100);
            Assert.True(TweetReceived);
        }
    }
}