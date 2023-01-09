using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Twitter.Core.Constants;
using Twitter.Service.Data;
using Twitter.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

DotNetEnv.Env.TraversePath().Load();

builder.Services.AddDbContextFactory<TwitterDbContext>(options =>
    options.UseSqlServer(Environment.GetEnvironmentVariable(TwitterConstants.DATABASE_CONNECTION)));

builder.Services.AddScoped<ITweetService, TweetService>();

builder.Services.AddScoped<IHashTagService, HashTagService>();

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
