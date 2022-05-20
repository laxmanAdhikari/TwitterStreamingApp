
using Microsoft.EntityFrameworkCore;
using Twitter.BlazorServer.Data;
using Twitter.BlazorServer.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<ITweetService, TweetService>();


var app = builder.Build();

// Use SQL Database 

DotNetEnv.Env.TraversePath().Load();

//builder.Services.AddDbContext<TwitterDbContext>( options =>
//{
//    options.UseSqlServer()
//});

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
