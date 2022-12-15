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
using Microsoft.Extensions.Options;

namespace B2CAzureFunc
{
    /// <summary>
    /// CheckDSSUSerExistence
    /// </summary>
    public class CheckDSSUSerExistence
    {
        private readonly AppSettings _appSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appSettings"></param>
        public CheckDSSUSerExistence(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings?.Value;
        }

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

                using (var httpClient = new HttpClient())
                {
                    var searchApiUrl = _appSettings.NcsDssSearchApiUrl;
                    var url = String.Format("{0}?&search=EmailAddress:{1}", searchApiUrl, data.Email);
                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), url))
                    {
                        request.Headers.TryAddWithoutValidation("api-key", _appSettings.NcsDssApiKey);
                        request.Headers.TryAddWithoutValidation("version", _appSettings.NcsDssSearchApiVersion);
                        request.Headers.TryAddWithoutValidation("Ocp-Apim-Subscription-Key", _appSettings:OcpApimSubKey);
                        request.Headers.TryAddWithoutValidation("TouchpointId", _appSettings.TouchpointId.ToString());

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
                log.LogError(ex.ToString());

                return new BadRequestObjectResult(new ResponseContentModel
                {
                    version = "1.0.0",
                    userMessage = "Sorry, Something happened unexpectedly. Please try after sometime.",
                    developerMessage = "See logging provider failure dependencies for exception information.",
                    status = 400
                });
            }
        }
    }
}
