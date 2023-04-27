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
using Microsoft.Extensions.Options;

namespace B2CAzureFunc
{
    /// <summary>
    ///     FindEmail
    /// </summary>
    public class FindEmail
    {

        private readonly AppSettings _appSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appSettings"></param>
        public FindEmail(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings?.Value;
        }

        /// <summary>
        ///     FindEmail
        /// </summary>
        /// <verb>POST</verb>
        /// <url>http://localhost:7070/FindEmail</url>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <response code="200"><see cref="EmailFoundResponseModel"/>Email Found Response</response>
        /// <response code="404"><see cref="Object"/>Not Found</response>
        [FunctionName("FindEmail")]
        public async Task<IActionResult> Run(
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
                    !String.IsNullOrEmpty(data.Day) && !String.IsNullOrEmpty(data.Month) && !String.IsNullOrEmpty(data.Year)
                    && !String.IsNullOrEmpty(data.PostalCode))
                {
                    string email = "";
                    using (var httpClient = new HttpClient())
                    {
                        var dob = String.Format("{0}-{1}-{2}", data.Year, data.Month, data.Day);
                        var searchApiUrl = _appSettings.NcsDssSearchApiUrl;
                        var url = String.Format("{0}?&search=GivenName:{1} FamilyName:{2} PostCode={3}&filter=DateOfBirth eq {4}",
                             searchApiUrl, data.GivenName, data.Surname, data.PostalCode, dob);
                        using (var request = new HttpRequestMessage(new HttpMethod("GET"), url))
                        {
                            request.Headers.TryAddWithoutValidation("api-key", _appSettings.NcsDssApiKey);
                            request.Headers.TryAddWithoutValidation("version", _appSettings.NcsDssSearchApiVersion);
                            request.Headers.TryAddWithoutValidation("Ocp-Apim-Subscription-Key", _appSettings.OcpApimSubscriptionKey);

                            var response = await httpClient.SendAsync(request);
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var result = JsonConvert.DeserializeObject<SearchAPIResponseModel>(await response.Content.ReadAsStringAsync());
                                if (result.Value.Length > 0)
                                {
                                    email = result.Value[0].EmailAddress != null ? result.Value[0].EmailAddress.ToString() : "";
                                }
                            }
                            else
                            {
                                return new BadRequestObjectResult(new ResponseContentModel
                                {
                                    userMessage = "Sorry, Something happened unexpectedly. Please try after sometime.",
                                    status = 400
                                });
                            }
                        }
                    }
                    if (!String.IsNullOrEmpty(email))
                    {
                        return new OkObjectResult(new EmailFoundResponseModel
                        {
                            foundEmail = email,
                            isFound = true
                        });
                    }
                    else
                    {
                        return new OkObjectResult(new EmailFoundResponseModel
                        {
                            foundEmail = "",
                            isFound = false
                        });
                    }
                }
                else
                {
                    return new OkObjectResult(new EmailFoundResponseModel
                    {
                        foundEmail = "",
                        isFound = false
                    });
                }
            }
            catch (Exception ex)
            {
                log.LogInformation(ex.ToString());

                return new BadRequestObjectResult(new ResponseContentModel
                {
                    userMessage = "Sorry, Something happened unexpectedly. Please try after sometime.",
                    status = 400
                });
            }
        }
    }
}