using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using B2CAzureFunc.Models;

namespace B2CAzureFunc
{
    /// <summary>
    /// NCSDSSUserCreation
    /// </summary>
    public static class NCSDSSUserCreation
    {
        /// <summary>
        ///     NCSDSSUserCreation
        /// </summary>
        /// <verb>POST</verb>
        /// <url>http://localhost:7070/NCSDSSUserCreation</url>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <response code="200"><see cref="UserCreationModel"/>User Created Response</response>
        /// <response code="404"><see cref="Object"/>Not Found</response>
        [FunctionName("NCSDSSUserCreation")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                UserCreationModel data = JsonConvert.DeserializeObject<UserCreationModel>(requestBody);
                log.LogInformation(requestBody);
                bool completionStatus = false;
                var userMessage = "";

                if (!data.IsAided)
                {
                    // Create cutomer
                    using (var httpClient = new HttpClient())
                    {
                        using (var request = new HttpRequestMessage(new HttpMethod("POST"), Environment.GetEnvironmentVariable("ncsdsscreatecustomerapiurl", EnvironmentVariableTarget.Process)))
                        {
                            request.Headers.TryAddWithoutValidation("api-key", Environment.GetEnvironmentVariable("ncsdssapikey", EnvironmentVariableTarget.Process));
                            request.Headers.TryAddWithoutValidation("OcpApimSubscriptionKey", Environment.GetEnvironmentVariable("OcpApimSubscriptionKey", EnvironmentVariableTarget.Process));
                            request.Headers.TryAddWithoutValidation("TouchpointId", Environment.GetEnvironmentVariable("TouchpointId", EnvironmentVariableTarget.Process));
                            var dob = "";
                            string payload = "";
                            if (!String.IsNullOrEmpty(data.Year) && !String.IsNullOrEmpty(data.Month) && !String.IsNullOrEmpty(data.Day))
                            {
                                dob = String.Format("{0}-{1}-{2}", data.Year, data.Month, data.Day);
                                payload = "{\n    \"GivenName\": \"" + data.GivenName + "\",\n    \"FamilyName\": \"" + data.Surname + "\",\n    \"PriorityGroups\": \"[98]\",\n    \"DateofBirth\":\"" + dob + "\"\n}";
                            }
                            else
                            {
                                payload = "{\n    \"GivenName\": \"" + data.GivenName + "\",\n    \"FamilyName\": \"" + data.Surname + "\",\n    \"PriorityGroups\": \"[98]\",\n }";
                            }

                            request.Content = new StringContent(payload);
                            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                            var response = await httpClient.SendAsync(request);

                            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                            {
                                var result = JsonConvert.DeserializeObject<UserCreationResponseModel>(await response.Content.ReadAsStringAsync());

                                if (result != null)
                                {
                                    data.CustomerId = result.CustomerId.ToString();
                                    completionStatus = true;
                                }
                                else
                                {
                                    return new BadRequestObjectResult(new ResponseContentModel
                                    {
                                        version = "1.0.0",
                                        userMessage = "Sorry, Something happened unexpectedly. Please try after sometime.",
                                        status = 400
                                    });
                                }
                            }
                            else
                            {
                                return new BadRequestObjectResult(new ResponseContentModel
                                {
                                    version = "1.0.0",
                                    userMessage = "Sorry, Something happened unexpectedly. Please try after sometime.",
                                    status = 400
                                });
                            }
                        }
                    }

                    // Create contact

                    using (var httpClient = new HttpClient())
                    {
                        var url = Environment.GetEnvironmentVariable("ncsdsscreatecontactapiurl", EnvironmentVariableTarget.Process);
                        url = String.Format(url, data.CustomerId);

                        using (var request = new HttpRequestMessage(new HttpMethod("POST"), url))
                        {
                            request.Headers.TryAddWithoutValidation("api-key", Environment.GetEnvironmentVariable("ncsdssapikey", EnvironmentVariableTarget.Process));
                            request.Headers.TryAddWithoutValidation("OcpApimSubscriptionKey", Environment.GetEnvironmentVariable("OcpApimSubscriptionKey", EnvironmentVariableTarget.Process));
                            request.Headers.TryAddWithoutValidation("TouchpointId", Environment.GetEnvironmentVariable("TouchpointId", EnvironmentVariableTarget.Process));

                            request.Content = new StringContent("{\n    \"EmailAddress\": \"" + data.Email + "\",\n    \"PreferredContactMethod\": \"1\"\n}");
                            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                            var response = await httpClient.SendAsync(request);

                            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                            {
                                var result = JsonConvert.DeserializeObject<ContactCreationResponseModel>(await response.Content.ReadAsStringAsync());

                                if (result == null)
                                    return new BadRequestObjectResult(new ResponseContentModel
                                    {
                                        version = "1.0.0",
                                        userMessage = "Sorry, Something happened unexpectedly. Please try after sometime.",
                                        status = 400
                                    });

                                completionStatus = true;
                            }
                            else
                            {
                                return new BadRequestObjectResult(new ResponseContentModel
                                {
                                    version = "1.0.0",
                                    userMessage = "Sorry, Something happened unexpectedly. Please try after sometime.",
                                    status = 400
                                });
                            }
                        }
                    }

                    // Create digital identity
                    using (var httpClient = new HttpClient())
                    {
                        using (var request = new HttpRequestMessage(new HttpMethod("POST"), Environment.GetEnvironmentVariable("ncsdsscreateidentityapiurl", EnvironmentVariableTarget.Process)))
                        {
                            request.Headers.TryAddWithoutValidation("api-key", Environment.GetEnvironmentVariable("ncsdssapikey", EnvironmentVariableTarget.Process));
                            request.Headers.TryAddWithoutValidation("OcpApimSubscriptionKey", Environment.GetEnvironmentVariable("OcpApimSubscriptionKey", EnvironmentVariableTarget.Process));
                            request.Headers.TryAddWithoutValidation("TouchpointId", Environment.GetEnvironmentVariable("TouchpointId", EnvironmentVariableTarget.Process));
                            request.Headers.TryAddWithoutValidation("version", Environment.GetEnvironmentVariable("ncsdsssearchapiversion", EnvironmentVariableTarget.Process));

                            request.Content = new StringContent("{\n    \"CustomerId\": \"" + data.CustomerId + "\",\n    \"IdentityStoreId\": \"" + data.ObjectId + "\"\n}");
                            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                            var response = await httpClient.SendAsync(request);
                            userMessage = await response.Content.ReadAsStringAsync();
                            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                            {
                                var result = JsonConvert.DeserializeObject<IdentityCreationResponseModel>(await response.Content.ReadAsStringAsync());

                                if (result == null)
                                    return new BadRequestObjectResult(new ResponseContentModel
                                    {
                                        version = "1.0.0",
                                        userMessage = "Sorry, Something happened unexpectedly. Please try after sometime.",
                                        status = 400,
                                        developerMessage = "response null, uri: " + request.RequestUri.ToString() + "===============" + userMessage
                                    });

                                completionStatus = true;
                            }
                            else
                            {
                                return new BadRequestObjectResult(new ResponseContentModel
                                {
                                    version = "1.0.0",
                                    userMessage = "Sorry, Something happened unexpectedly. Please try after sometime.",
                                    status = 400,
                                    developerMessage = "response.StatusCode: " + response.StatusCode + ", uri: " + request.RequestUri.ToString() + "===============" + userMessage
                                });
                            }
                        }
                    }

                }
                else
                {
                    // Update digital identity
                    using (var httpClient = new HttpClient())
                    {

                        var patchApiUrl = Environment.GetEnvironmentVariable("ncsdsspatchdigitalidentityapiurl", EnvironmentVariableTarget.Process);
                        var requestUrl = String.Format(patchApiUrl, data.CustomerId);

                        using (var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUrl))
                        {
                            request.Headers.TryAddWithoutValidation("api-key", Environment.GetEnvironmentVariable("ncsdssapikey", EnvironmentVariableTarget.Process));
                            request.Headers.TryAddWithoutValidation("OcpApimSubscriptionKey", Environment.GetEnvironmentVariable("OcpApimSubscriptionKey", EnvironmentVariableTarget.Process));
                            request.Headers.TryAddWithoutValidation("TouchpointId", Environment.GetEnvironmentVariable("TouchpointId", EnvironmentVariableTarget.Process));
                            request.Headers.TryAddWithoutValidation("version", Environment.GetEnvironmentVariable("ncsdsssearchapiversion", EnvironmentVariableTarget.Process));

                            request.Content = new StringContent("{\n    \"CustomerId\": \"" + data.CustomerId + "\",\n    \"IdentityStoreId\": \"" + data.ObjectId + "\"\n}");
                            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                            var response = await httpClient.SendAsync(request);
                            userMessage = await response.Content.ReadAsStringAsync();
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                //var result = JsonConvert.DeserializeObject<DigitalIdentityUpdateResponseModel>(await response.Content.ReadAsStringAsync());

                                //if (result == null)
                                //    return new BadRequestObjectResult(new ResponseContentModel
                                //    {
                                //        version = "1.0.0",
                                //        userMessage = "Sorry, Something happened unexpectedly. Please try after sometime.",
                                //        status = 400,
                                //        developerMessage = "response null, uri: " + request.RequestUri.ToString() + "===============" + userMessage
                                //    });

                                completionStatus = true;
                            }
                            else
                            {
                                return new BadRequestObjectResult(new ResponseContentModel
                                {
                                    version = "1.0.0",
                                    userMessage = "Sorry, Something happened unexpectedly. Please try after sometime.",
                                    status = 400,
                                    developerMessage = "response.StatusCode: " + response.StatusCode + ", uri: " + request.RequestUri.ToString() + "===============" + userMessage
                                });
                            }
                        }
                    }
                }

                if (completionStatus)
                {
                    return new OkObjectResult(new { customerId = data.CustomerId });
                }
                else
                {
                    return new BadRequestObjectResult(new ResponseContentModel
                    {
                        version = "1.0.0",
                        userMessage = "Sorry, Something happened unexpectedly. Please try after sometime.",
                        status = 400,
                        developerMessage = "completionStatus: " + completionStatus + "===============" + userMessage
                    });
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