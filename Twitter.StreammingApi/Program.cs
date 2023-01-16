
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using TwitterStream.Api;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ServicesDependencyInjection();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddApiVersioning( opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions= true;    
});

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl= true;

});

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
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwagger();

    app.UseSwaggerUI( options =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });

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

//app.MapHub<TwitterStreamHub>("/twitter-hub");

app.MapControllers();

app.UseCors("TweetPolicy");

app.Run();

public partial class Program { }