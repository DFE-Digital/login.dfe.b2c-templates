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
using System.Net.Http;
using System.Linq;

namespace B2CAzureFunc
{
    /// <summary>
    /// CheckDSSUSerExistence
    /// </summary>
    public static class CheckDSSUSerExistence
    {
        /// <summary>
        ///     CheckDSSUSerExistence
        /// </summary>
        /// <verb>POST</verb>
        /// <url>http://localhost:7070/CheckDSSUSerExistence</url>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <response code="200"><see cref="ResponseContentModel"/>User Found Response</response>
        /// <response code="404"><see cref="Object"/>Not Found</response>
        /// <response code="409"><see cref="ResponseContentModel"/>Not Found</response>
        [FunctionName("CheckDSSUSerExistence")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                log.LogInformation("Request started");
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                FindEmailModel data = JsonConvert.DeserializeObject<FindEmailModel>(requestBody);
                log.LogInformation(requestBody);

                using (var httpClient = new HttpClient())
                {
                    var searchApiUrl = Environment.GetEnvironmentVariable("ncs-dss-search-api-url", EnvironmentVariableTarget.Process);
                    var url = String.Format("{0}?&search=EmailAddress:{1}", searchApiUrl, data.Email);
                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), url))
                    {
                        request.Headers.TryAddWithoutValidation("api-key", Environment.GetEnvironmentVariable("ncs-dss-api-key", EnvironmentVariableTarget.Process));
                        request.Headers.TryAddWithoutValidation("version", Environment.GetEnvironmentVariable("ncs-dss-search-api-version", EnvironmentVariableTarget.Process));
                        request.Headers.TryAddWithoutValidation("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("Ocp-Apim-Subscription-Key", EnvironmentVariableTarget.Process));

                        var response = await httpClient.SendAsync(request);
                        var retryConter = Convert.ToInt32(data.RetryCounter);
                        ++retryConter;
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var result = JsonConvert.DeserializeObject<SearchAPIResponseModel>(await response.Content.ReadAsStringAsync());
                            var value = result.Value.FirstOrDefault(p => p.EmailAddress.ToString().ToLower() == data.Email.ToLower());
                            if (value != null)
                            {
                                return new OkObjectResult(new ResponseContentModel
                                {
                                    version = "1.0.0",
                                    status = 200,
                                    isFound = true
                                });
                            }
                            else
                            {
                                return new OkObjectResult(new ResponseContentModel
                                {
                                    version = "1.0.0",
                                    status = 200,
                                    isFound = false
                                });
                            }
                        }
                        else
                        {
                            return new BadRequestObjectResult(new ResponseContentModel
                            {
                                version = "1.0.0",
                                userMessage = "Sorry, Something happened unexpectedly. Please try after sometime.",
                                status = 400,
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new ResponseContentModel
                {
                    version = "1.0.0",
                    userMessage = "Sorry, Something happened unexpectedly. Please try after sometime.",
                    developerMessage = ex.ToString(),
                    status = 400
                });
            }
        }
    }
}
