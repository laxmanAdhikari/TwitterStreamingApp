namespace Twitter.BackgroundWorker.Jobs
{
    public interface ITwitterIntegrationJobService
    {
       Task RunJobAsync(TwitterIntegrationJob twitterIntegrationJob);
    }
}
