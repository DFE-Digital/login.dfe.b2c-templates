using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using B2CAzureFunc.Helpers;
using B2CAzureFunc.Models;
using System.IO;
using Newtonsoft.Json;

namespace B2CAzureFunc
{
    /// <summary>
    /// DeleteUser
    /// </summary>
    public static class DeleteUser
    {
        /// <summary>
        /// DeleteUser
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <param name="id"></param>
        /// <returns>OkObjectResult</returns>
        [FunctionName("DeleteUser")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string id = req.Query["id"];
                if (!String.IsNullOrEmpty(id))
                {
                    string tenant = Environment.GetEnvironmentVariable("b2c:Tenant", EnvironmentVariableTarget.Process);
                    string clientId = Environment.GetEnvironmentVariable("b2c:GraphAccessClientId", EnvironmentVariableTarget.Process);
                    string clientSecret = Environment.GetEnvironmentVariable("b2c:GraphAccessClientSecret", EnvironmentVariableTarget.Process);
                    B2CGraphClient client = new B2CGraphClient(clientId, clientSecret, tenant);

                    var status = await client.DeleteUser(id);
                    if (status)
                    {
                        return (ActionResult)new OkObjectResult(status);
                    }
                    else
                    {
                        return new BadRequestObjectResult(new ResponseContentModel
                        {
                            version = "1.0.0",
                            userMessage = "Sorry, something happened unexpectedly. Couldn't delete the user. Please try again later.",
                            status = 409
                        });
                    }
                }
                else
                {
                    return new BadRequestObjectResult(new ResponseContentModel
                    {
                        version = "1.0.0",
                        userMessage = "Please pass object id of the user",
                        status = 409
                    });
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new ResponseContentModel
                {
                    version = "1.0.0",
                    userMessage = "Sorry, something happened unexpectedly. Couldn't delete the user. Please try again later.",
                    status = 400,
                    developerMessage = ex.ToString()
                });
            }
        }
    }
}