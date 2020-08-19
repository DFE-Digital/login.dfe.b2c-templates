using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using B2CAzureFunc.Models;
using System;
using B2CAzureFunc.Helpers;
using System.Linq;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;

namespace B2CAzureFunc
{
    /// <summary>
    /// FindAccount
    /// </summary>
    public static class FindAccount
    {
        /// <summary>
        /// FindAccount
        /// </summary>
        /// <verb>GET</verb>
        /// <url>http://localhost:7070/FindAccount</url>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <param name="context"></param>
        /// <response code="400"><see cref="ResponseContentModel"/>Not Found</response>
        /// <response code="200"><see cref="object"/>Account Found Response</response>
        [FunctionName("FindAccount")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
            try
            {
                string id = req.Query["id"];
                log.LogInformation("Query: " + req.Query);
                log.LogInformation(id);

                if (!String.IsNullOrEmpty(id))
                {
                    string tenant = ConfigurationHelper.GetConfigurationValue(context, "AppSettings:b2c:Tenant");
                    string clientId = ConfigurationHelper.GetConfigurationValue(context, "AppSettings:b2c:GraphAccessClientId");
                    string clientSecret = ConfigurationHelper.GetConfigurationValue(context, "AppSettings:b2c:GraphAccessClientSecret");
                    string extensionAppId = ConfigurationHelper.GetConfigurationValue(context, "AppSettings:ExtensionAppId");

                    B2CGraphClient client = new B2CGraphClient(clientId, clientSecret, tenant);

                    var getUserApiResponse = await client.GetUserByObjectId(id);
                    if (!String.IsNullOrEmpty(getUserApiResponse))
                    {
                        var user = JsonConvert.DeserializeObject<UserValueModel>(getUserApiResponse);
                        if (user == null || String.IsNullOrEmpty(user.objectId))
                        {
                            return new BadRequestObjectResult(new ResponseContentModel
                            {
                                userMessage = "No such a user exist. Please check the Object Id"
                            });
                        }
                        else
                        {
                            JObject obj = JObject.Parse(getUserApiResponse);
                            var customerId = obj["extension_" + extensionAppId + "_customerId"];
                            return (ActionResult)new OkObjectResult(new { FirstName = user.givenName, LastName = user.surname, DisplayName = user.displayName, Email = user.signInNames.FirstOrDefault().value, CustomerId = customerId });
                        }
                    }
                    else
                    {
                        return new BadRequestObjectResult(new ResponseContentModel
                        {
                            userMessage = "No such a user exist. Please check the Object Id"
                        });
                    }
                }
                else
                {
                    return new BadRequestObjectResult(new ResponseContentModel
                    {
                        userMessage = "Invalid input, Object Id cannot be null"
                    });
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new ResponseContentModel
                {
                    userMessage = "Sorry, something happened unexpectedly. Please try again later.",
                    developerMessage = ex.ToString()
                });
            }
        }
    }
}