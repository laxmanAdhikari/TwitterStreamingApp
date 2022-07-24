namespace TwitterStreamApi.Background.Jobs
{
    public interface ITwitterIntegrationJobService
    {
       Task RunJobAsync(TwitterIntegrationJob twitterIntegrationJob);
    }
}
