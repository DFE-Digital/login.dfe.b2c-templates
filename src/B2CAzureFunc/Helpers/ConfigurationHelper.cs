using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2CAzureFunc.Helpers
{
    /// <summary>
    /// ConfigurationHelper
    /// </summary>
    public class ConfigurationHelper
    {
        /// <summary>
        /// GetConfigurationValue
        /// </summary>
        /// <param name="context"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigurationValue(ExecutionContext context, string key)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            return config[key];
        }
    }
}