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
    ///     AidedRegistrationValidateUserDetails
    /// </summary>
    public static class AidedRegistrationValidateUserDetails
    {
        /// <summary>
        ///     AidedRegistrationValidateUserDetails
        /// </summary>
        /// <verb>POST</verb>
        /// <url>http://localhost:7070/api/FindEmail</url>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <response code="200"><see cref="EmailFoundResponseModel"/>Email Found Response</response>
        /// <response code="404"><see cref="Object"/>Not Found</response>
        [FunctionName("AidedRegistrationValidateUserDetails")]
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

                if (!String.IsNullOrEmpty(data.GivenName) && !String.IsNullOrEmpty(data.Surname) &&
                    !String.IsNullOrEmpty(data.Day) && !String.IsNullOrEmpty(data.Month) && !String.IsNullOrEmpty(data.Year))
                {
                    using (var httpClient = new HttpClient())
                    {
                        var dob = String.Format("{0}-{1}-{2}", data.Year, data.Month, data.Day);
                        var searchApiUrl = Environment.GetEnvironmentVariable("ncs-dss-search-api-url", EnvironmentVariableTarget.Process);
                        var url = String.Format("{0}?&search=EmailAddress:{1}",
                             searchApiUrl, data.Email);
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

                                    log.LogInformation("Param: " + dob);

                                    log.LogInformation("API: " + value.DateOfBirth);


                                    var day = value.DateOfBirth.Day;
                                    var month = value.DateOfBirth.Month;
                                    var year = value.DateOfBirth.Year;

                                    var dayFromParam = Convert.ToInt32(data.Day);
                                    var monthFromParam = Convert.ToInt32(data.Month);
                                    var yearFromParam = Convert.ToInt32(data.Year);

                                    if (day == dayFromParam && month == monthFromParam && year == yearFromParam
                                        && data.GivenName.ToLower() == value.GivenName.ToLower()
                                        && data.Surname.ToLower() == value.FamilyName.ToLower())
                                    {
                                        return new OkObjectResult(new
                                        {
                                            isFound = true,
                                        });
                                    }
                                    else
                                    {
                                        return new BadRequestObjectResult(new ResponseContentModel
                                        {
                                            version= "1.0.0",
                                            userMessage = "Your date of birth does not match your account details.",
                                            isFound = false,
                                            status = 409
                                        });
                                    }
                                }
                                else
                                {
                                    return new BadRequestObjectResult(new ResponseContentModel
                                    {
                                        version = "1.0.0",
                                        userMessage = "Sorry, Please provide valid information ",
                                        status = 409,
                                    });
                                }
                            }
                            else
                            {
                                return new BadRequestObjectResult(new ResponseContentModel
                                {
                                    version = "1.0.0",
                                    userMessage = "Sorry, Something happened unexpectedly. Please try after sometime.",
                                    status = 409,
                                });
                            }
                        }
                    }
                }
                else
                {
                    return new BadRequestObjectResult(new ResponseContentModel
                    {
                        version = "1.0.0",
                        userMessage = "Your date of birth does not match your account details.",
                        status = 409,
                    });
                }
            }
            catch (Exception ex)
            {
                log.LogInformation(ex.ToString());

                return new BadRequestObjectResult(new ResponseContentModel
                {
                    version = "1.0.0",
                    userMessage = "Sorry, Something happened unexpectedly. Please try after sometime.",
                    status = 409
                });
            }
        }
    }
}
