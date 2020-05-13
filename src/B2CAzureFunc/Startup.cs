using B2CAzureFunc.Config;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

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
            builder.Services.AddOptions<B2COpenApiConfig>()
                .Configure<IConfiguration>((openApiConfig, config) =>
                {
                    config.GetSection("B2COpenApiConfig").Bind(openApiConfig);
                });

            var logger = new LoggerConfiguration().CreateLogger();
            builder.Services.AddLogging(lb => lb.AddSerilog(logger));
        }
    }
}
