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
using Microsoft.Extensions.Options;

namespace B2CAzureFunc
{
    /// <summary>
    /// FindAccount
    /// </summary>
    public class FindAccount
    {

        private readonly AppSettings _appSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appSettings"></param>
        public FindAccount(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings?.Value;
        }
        /// <summary>
        /// FindAccount
        /// </summary>
        /// <verb>GET</verb>
        /// <url>http://localhost:7070/FindAccount</url>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <response code="400"><see cref="ResponseContentModel"/>Not Found</response>
        /// <response code="200"><see cref="object"/>Account Found Response</response>
        [FunctionName("FindAccount")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string id = req.Query["id"];
                log.LogInformation("Query: " + req.Query);
                log.LogInformation(id);

                if (!String.IsNullOrEmpty(id))
                {
                    string tenant = _appSettings.B2CTenantId;
                    string clientId = _appSettings.B2CGraphAccessClientId.ToString();
                    string clientSecret = _appSettings.B2CGraphAccessClientSecret;
                    string extensionAppId = _appSettings.ExtensionAppId;
                    log.LogInformation("tenant: " + tenant);

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