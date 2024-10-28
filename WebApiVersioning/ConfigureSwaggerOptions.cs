using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApiVersioning
{
    // This class configures Swagger options based on the API version descriptions.
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;

        // Constructor that takes an IApiVersionDescriptionProvider to get API version info
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _apiVersionDescriptionProvider = provider;
        }

        // This method is called to configure Swagger options
        public void Configure(SwaggerGenOptions options)
        {
            // Loop through each API version description to set up Swagger documentation
            foreach (var description in _apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                // Register a Swagger document for each version of the API
                options.SwaggerDoc(description.GroupName, CreateInfoVersion(description));
            }
        }

        // This method creates the OpenApiInfo object that contains metadata about the API version
        private OpenApiInfo CreateInfoVersion(ApiVersionDescription description)
        {
            return new OpenApiInfo
            {
                Title = "My API",  // Title of the API
                Version = description.ApiVersion.ToString()  // API version string (e.g., "1.0", "2.0")
            };
        }
    }
}
