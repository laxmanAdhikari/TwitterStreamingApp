using Microsoft.AspNetCore.SignalR;

namespace Twitter.BlazorServer.Hubs
{
    public class BroadcastHub : Hub
    {
        public async Task SendMessage()
        {
            await Clients.All.SendAsync("TweetReceived");
        }
    }
}
