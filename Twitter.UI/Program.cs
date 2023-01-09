using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using Twitter.Core.Constants;
using Twitter.Service.Data;
using Twitter.Service.Services;
using Twitter.UI;
using static Humanizer.In;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddHttpClient();

DotNetEnv.Env.TraversePath().Load();
builder.Services.AddDbContextFactory<TwitterDbContext>(options =>
   options.UseSqlServer("server=(local)\\SqlExpress;Database=TweetStreaming;Trusted_Connection = Yes"));


builder.Services.AddScoped<ITweetService, TweetService>();


await builder.Build().RunAsync();
