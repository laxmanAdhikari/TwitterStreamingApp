using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using Twitter.BackgroundWorker;
using Twitter.BackgroundWorker.Background;
using Twitter.BackgroundWorker.Jobs;
using Twitter.Core.Constants;
using Twitter.Core.Services;
using Twitter.Data;
using Twitter.Service.Mappings;
using Twitter.Service.Services;

using IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "Twitter Streaming Service";
    })
    .ConfigureServices(services =>
    {
        LoggerProviderOptions.RegisterProviderOptions<
            EventLogSettings, EventLogLoggerProvider>(services);

        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<ITweetService, TweetService>();
        services.AddScoped<IHashTagService, HashTagService>();
        services.AddSingleton<ITwitterApiTweetService, TwitterApiTweetService>();
        services.AddHostedService<TwitterIntegrationBackgroundService>();
        services.AddSingleton<ITwitterIntegrationJobService, TwitterIntegrationJobService>();
        services.AddSignalRCore();
        DotNetEnv.Env.TraversePath().Load();
        services.AddDbContext<TwitterDbContext>(options =>
           options.UseSqlServer(Environment.GetEnvironmentVariable(TwitterConstants.DATABASE_CONNECTION)));
        services.AddHttpClient();
        services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
    })
    .ConfigureLogging((context, logging) =>
    {
        // See: https://github.com/dotnet/runtime/issues/47303
        logging.AddConfiguration(
            context.Configuration.GetSection("Logging"));
    })
    .Build();

await host.RunAsync();
