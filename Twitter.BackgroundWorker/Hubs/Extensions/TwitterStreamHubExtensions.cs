using Microsoft.AspNetCore.SignalR;
using Twitter.Model.Results;

namespace Twitter.BackgroundWorker.Hubs.Extensions
{
    public  static class TwitterStreamHubExtensions
    {
        public static async Task CreateTweetAsync(this IHubContext<TwitterStreamHub> twitterStream, CreateTweetResults createTweetResult, CancellationToken cancellationToken)
        {
            await twitterStream.Clients.All.SendAsync("createTweet", createTweetResult, cancellationToken);
        }
    }
}
