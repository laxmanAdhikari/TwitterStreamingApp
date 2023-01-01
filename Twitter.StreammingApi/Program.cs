
WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ServicesDependencyInjection();
builder.Services.Configure<ApiConfiguration>(builder.Configuration.GetSection("Tweets"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("TweetPolicy", builder =>
    {
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
        builder.AllowAnyOrigin();
    });
});

var app = builder.Build();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Uncomment to use the open API specification. The SwaggerUI behaves funcky
    //app.UseSwaggerUI( opt =>
    //{
    //    opt.SwaggerEndpoint("/tweet-api-spec.json", "Twitter streaming API conversion");
    //});
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapDefaultControllerRoute();

app.UseMiddleware<ErrorHandling>();

app.MapControllers();

app.MapHub<TwitterStreamHub>("/twitter-hub");

app.MapControllers();

app.UseCors("TweetPolicy");

app.Run();

public partial class Program { }