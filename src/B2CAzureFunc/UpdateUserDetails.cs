using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using B2CAzureFunc.Models;
using B2CAzureFunc.Helpers;
using System.Linq;

namespace B2CAzureFunc
{
    /// <summary>
    /// UpdateUser
    /// </summary>
    public static class UpdateUser
    {
        /// <summary>
        /// UpdateUser
        /// </summary>
        /// <verb>PUT</verb>
        /// <url>http://localhost:7070/UpdateUser</url>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <response code="200"><see cref="bool"/>User Details Updated Response</response>
        /// <response code="400"><see cref="ResponseContentModel"/>Update Error</response>
        /// <response code="400"><see cref="ResponseContentModel"/>Bad request response</response>
        [FunctionName("UpdateUser")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                UserProfileModel data = JsonConvert.DeserializeObject<UserProfileModel>(requestBody);
                log.LogInformation(requestBody);

                if (data != null)
                {
                    if (String.IsNullOrEmpty(data.ObjectId))
                    {
                        return new BadRequestObjectResult(new ResponseContentModel
                        {
                            version = "1.0.0",
                            userMessage = "Object id can't be null"
                        });
                    }

                    if (String.IsNullOrEmpty(data.DisplayName))
                        data.DisplayName = data.FirstName + " " + data.LastName;

                    string tenant = Environment.GetEnvironmentVariable("b2c:Tenant", EnvironmentVariableTarget.Process);
                    string clientId = Environment.GetEnvironmentVariable("b2c:GraphAccessClientId", EnvironmentVariableTarget.Process);
                    string clientSecret = Environment.GetEnvironmentVariable("b2c:GraphAccessClientSecret", EnvironmentVariableTarget.Process);
                    B2CGraphClient client = new B2CGraphClient(clientId, clientSecret, tenant);

                    var getUserApiResponse = await client.GetUserByObjectId(data.ObjectId);
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

                    var status = await client.UpdateUser(data.ObjectId, JsonConvert.SerializeObject(new { givenName = data.FirstName, surname = data.LastName, displayName = data.DisplayName }));
                    if (status)
                    {
                        return (ActionResult)new OkObjectResult(status);
                    }
                    else
                    {
                        return new BadRequestObjectResult(new ResponseContentModel
                        {
                            userMessage = "Sorry, something happened unexpectedly. Couldn't update the user. Please try again later."
                        });
                    }
                }
                else
                {
                    return new BadRequestObjectResult(new ResponseContentModel
                    {
                        userMessage = "Please provide valid input"
                    });
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new ResponseContentModel
                {
                    userMessage = "Sorry, something happened unexpectedly. Couldn't update the user. Please try again later."
                });
            }
        }
    }
}