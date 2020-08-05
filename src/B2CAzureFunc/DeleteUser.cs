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
        ///     DeleteUser
        /// </summary>
        /// <verb>DELETE</verb>
        /// <url>http://localhost:7070/DeleteUser</url>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <response code="200"><see cref="object"/>User deleted response</response>
        /// <response code="404"><see cref="ResponseContentModel"/>Not Found</response>
        /// <response code="409"><see cref="ResponseContentModel"/>Bad request response</response>
        [FunctionName("DeleteUser")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string id = req.Query["id"];
                log.LogInformation("Query: "+req.Query);
                log.LogInformation(id);
                if (!String.IsNullOrEmpty(id))
                {
                    string tenant = Environment.GetEnvironmentVariable("b2c:Tenant", EnvironmentVariableTarget.Process);
                    string clientId = Environment.GetEnvironmentVariable("b2c:GraphAccessClientId", EnvironmentVariableTarget.Process);
                    string clientSecret = Environment.GetEnvironmentVariable("b2c:GraphAccessClientSecret", EnvironmentVariableTarget.Process);
                    B2CGraphClient client = new B2CGraphClient(clientId, clientSecret, tenant);

                    var getUserApiResponse = await client.GetUserByObjectId(id);
                    if (!String.IsNullOrEmpty(getUserApiResponse))
                    {
                        var user = JsonConvert.DeserializeObject<UserValueModel>(getUserApiResponse);
                        if (user == null || String.IsNullOrEmpty(user.objectId))
                        {
                            return new BadRequestObjectResult(new ResponseContentModel
                            {
                                userMessage = "No such a user exist. Please check the Object Id",
                            });
                        }
                    }
                    else
                    {
                        return new BadRequestObjectResult(new ResponseContentModel
                        {
                            userMessage = "No such a user exist. Please check the Object Id",
                        });
                    }

                    var status = await client.DeleteUser(id);
                    if (status)
                    {
                        return (ActionResult)new OkObjectResult(status);
                    }
                    else
                    {
                        return new BadRequestObjectResult(new ResponseContentModel
                        {
                            userMessage = "Sorry, something happened unexpectedly. Couldn't delete the user. Please try again later."
                        });
                    }
                }
                else
                {
                    return new BadRequestObjectResult(new ResponseContentModel
                    {
                        userMessage = "Please pass object id of the user",
                    });
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new ResponseContentModel
                {
                    userMessage = "Sorry, something happened unexpectedly. Couldn't delete the user. Please try again later.",
                    developerMessage = ex.ToString()
                });
            }
        }
    }
}