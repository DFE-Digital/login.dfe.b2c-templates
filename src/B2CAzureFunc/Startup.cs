using System;
using System.IO;
using System.Reflection;
using B2CAzureFunc.Config;
using B2CAzureFunc.Models;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(B2CAzureFunc.Startup))]

namespace B2CAzureFunc
{
    /// <summary>
    /// Function Startup Bootstrapper
    /// </summary>
    public class Startup : FunctionsStartup
    {
        /// <summary>
        /// Function Configure Method
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("local.settings.json", true)
                   .AddEnvironmentVariables()
                   .Build();

            builder.Services.AddOptions<B2COpenApiConfig>()
                .Configure<IConfiguration>((openApiConfig, config) =>
                {
                    config.GetSection("B2COpenApiConfig").Bind(openApiConfig);
                });

            builder.Services.Configure<AppSettings>(config.GetSection("AppSettings"));

            builder.Services.AddOptions();
        }
    }
}