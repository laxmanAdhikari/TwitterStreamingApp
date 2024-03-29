﻿using Twitter.Data;
using Twitter.Service.Services;

namespace Twitter.StreamApi.Extentions
{
    public static class ApplicationDependencyExtensions
    {
        public static IServiceCollection ServicesDependencyInjection(this IServiceCollection services)
        {

            // Add services to the container.
            services.AddControllers();
            services.AddEndpointsApiExplorer();

            services.AddScoped<ITwitterApiTweetService, TwitterApiTweetService>();

            // Use SQL Database 
            DotNetEnv.Env.TraversePath().Load();
            services.AddDbContext<TwitterDbContext>(options =>
               options.UseSqlServer(Environment.GetEnvironmentVariable(TwitterConstants.DATABASE_CONNECTION)));

            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<ITweetService, TweetService>();
            services.AddScoped<IHashTagService, HashTagService>();

            //services.AddSingleton<ITwitterApiTweetService, TwitterApiTweetService>();

            //services.AddHostedService<TwitterIntegrationBackgroundService>();
            //services.AddSingleton<ITwitterIntegrationJobService, TwitterIntegrationJobService>();

            // Register IHttpFactory
            services.AddHttpClient();

            services.AddSignalR();

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "TwitterStreamApi", Version = "v1" });
                opt.CustomSchemaIds(type => type.FullName);
            });

            return services;
        }
    }
}
