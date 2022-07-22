using Microsoft.AspNetCore.SignalR;
using Twitter.Model.Results;

namespace TwitterStreamApiHelper.Hubs.Extensions
{
    public  static class TwitterStreamHubExtensions
    {
        public static async Task RecentTweetsSyncAsync(this IHubContext<TwitterStreamHub> twitterStream, bool refresh, CancellationToken cancellationToken)
        {
            await twitterStream.Clients.All.SendAsync("recentTweetSync", refresh, cancellationToken);
        }

        public static async Task CreateTweetAsync(this IHubContext<TwitterStreamHub> twitterStream, CreateTweetResults createTweetResult, CancellationToken cancellationToken)
        {
            await twitterStream.Clients.All.SendAsync("createTweet", createTweetResult, cancellationToken);
        }
    }
}
