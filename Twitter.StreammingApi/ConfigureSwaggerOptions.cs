using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TwitterStream.Api
{
    public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
                _provider= provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach(var description in _provider.ApiVersionDescriptions)
            {
                if (description.GroupName != "v1")
                    options.SwaggerDoc(
                        description.GroupName,
                        CreateVersionInfo(description));
            }
        }

        public void Configure(string name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        private OpenApiInfo CreateVersionInfo( ApiVersionDescription desc)
        {
            var info = new OpenApiInfo()
            {
                Title = "Twitter Streamming Ap using .Net 7 web Api",
                Version = desc.ApiVersion.ToString()
            };

            if(desc.IsDeprecated)
            {
                info.Description += "This API version has been deprecated. Please use one of the new APIs available from the explorer.";
            }
            return info;
        }

        
    }
}
