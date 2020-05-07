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

namespace B2CAzureFunc
{
    public static class FindEmail
    {
        [FunctionName("FindEmail")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                log.LogInformation("Request started");
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                FindEmailModel data = JsonConvert.DeserializeObject<FindEmailModel>(requestBody);
                log.LogInformation(requestBody);

                if (!String.IsNullOrEmpty(data.GivenName) && !String.IsNullOrEmpty(data.SurName) &&
                    !String.IsNullOrEmpty(data.Day) && !String.IsNullOrEmpty(data.Month) && !String.IsNullOrEmpty(data.Year)
                    && !String.IsNullOrEmpty(data.PostalCode))
                {
                    return new OkObjectResult(new
                    {
                        foundEmail = "amangupta@yopmail.com",
                    });
                }

                return new OkObjectResult(new
                {
                    foundEmail = "",
                });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new
                {
                    userMessage = "Sorry, Something happened unexpectedly. Please try after sometime.",
                    status = 404
                });
            }
        }
    }
}