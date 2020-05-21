using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using B2CAzureFunc.Helpers;
using Providers.Email.Model;
using Providers.Email;
using B2CAzureFunc.Models;
using System.Net.Http;
using System.Linq;

namespace B2CAzureFunc
{
    /// <summary>
    ///     SignUpInvitation
    /// </summary>
    public static class SignUpInvitation
    {
        /// <summary>
        ///     SignUpInvitation
        /// </summary>
        /// <verb>POST</verb>
        /// <url>http://localhost:7070/api/SignUpInvitation</url>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <response code="200"><see cref="bool"/>Invitation Sent</response>
        /// <response code="404"><see cref="Object"/>Error</response>
        [FunctionName("SignUpInvitation")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("Request started");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<SignupInvitationModel>(requestBody);
                log.LogInformation(requestBody);

                using (var httpClient = new HttpClient())
                {
                    var searchApiUrl = Environment.GetEnvironmentVariable("ncs-dss-search-api-url", EnvironmentVariableTarget.Process);
                    var searcUrl = String.Format("{0}?&search=EmailAddress:{1}",
                         searchApiUrl, data.Email);
                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), searcUrl))
                    {
                        request.Headers.TryAddWithoutValidation("api-key", Environment.GetEnvironmentVariable("ncs-dss-api-key", EnvironmentVariableTarget.Process));
                        request.Headers.TryAddWithoutValidation("version", Environment.GetEnvironmentVariable("ncs-dss-search-api-version", EnvironmentVariableTarget.Process));
                        request.Headers.TryAddWithoutValidation("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("Ocp-Apim-Subscription-Key", EnvironmentVariableTarget.Process));

                        var response = await httpClient.SendAsync(request);
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var searchResult = JsonConvert.DeserializeObject<SearchAPIResponseModel>(await response.Content.ReadAsStringAsync());
                            var value = searchResult.Value.FirstOrDefault(p => p.EmailAddress.ToString().ToLower() == data.Email.ToLower());
                            if (value == null)
                            {
                                return new BadRequestObjectResult(new ResponseContentModel
                                {
                                    version = "1.0.0",
                                    userMessage = "We have not been able to find your account",
                                    status = 409,
                                });
                            }
                            else
                            {
                                var accountActivationEmailExpiryInSeconds = Convert.ToInt32(Environment.GetEnvironmentVariable("AccountActivationEmailExpiryInSeconds", EnvironmentVariableTarget.Process));

                                string token = TokenBuilder.BuildIdToken(data.Email.ToString(), data.GivenName.ToString(), data.LastName.ToString(), DateTime.UtcNow.AddSeconds(accountActivationEmailExpiryInSeconds), req.Scheme, req.Host.Value, req.PathBase.Value);
                                string b2cURL = Environment.GetEnvironmentVariable("B2CAuthorizationUrl", EnvironmentVariableTarget.Process);
                                string b2cTenant = Environment.GetEnvironmentVariable("B2CTenant", EnvironmentVariableTarget.Process);
                                string b2cPolicyId = Environment.GetEnvironmentVariable("B2CSignUpPolicy", EnvironmentVariableTarget.Process);
                                string b2cClientId = Environment.GetEnvironmentVariable("B2CClientId", EnvironmentVariableTarget.Process);
                                string b2cRedirectUri = Environment.GetEnvironmentVariable("B2CRedirectUri", EnvironmentVariableTarget.Process);
                                string url = UrlBuilder.BuildUrl(token, b2cURL, b2cTenant, b2cPolicyId, b2cClientId, b2cRedirectUri);

                                string htmlTemplate = System.IO.File.ReadAllText(@"D:\home\site\wwwroot\AidedRegistrationEmailTemplate.html");
                                string from = Environment.GetEnvironmentVariable("SMTPFromAddress", EnvironmentVariableTarget.Process);
                                string subject = Environment.GetEnvironmentVariable("SignupEmailSubject", EnvironmentVariableTarget.Process);

                                htmlTemplate = htmlTemplate.Replace("#name#", data.GivenName.ToString()).Replace("#link#", url);

                                EmailModel model = new EmailModel
                                {
                                    Content = url,
                                    EmailTemplate = htmlTemplate,
                                    From = from,
                                    Subject = subject,
                                    To = data.Email.ToString()
                                };

                                var result = EmailService.SendEmail(model);
                                return result
                                    ? (ActionResult)new OkObjectResult(true)
                                    : new BadRequestObjectResult(new
                                    {
                                        userMessage = "Something happened unexpectedly.",
                                        version = "1.0.0",
                                        status = 409,
                                        code = "API12345",
                                        requestId = "50f0bd91-2ff4-4b8f-828f-00f170519ddb",
                                        developerMessage = "Verbose description of problem and how to fix it.",
                                        moreInfo = "https://restapi/error/API12345/moreinfo"
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
            catch (Exception ex)
            {
                log.LogInformation(ex.ToString());
                return new BadRequestObjectResult(new
                {
                    userMessage = ex.ToString(),
                    version = "1.0.0",
                    status = 409,
                    code = "API12345",
                    requestId = "50f0bd91-2ff4-4b8f-828f-00f170519ddb",
                    developerMessage = "Verbose description of problem and how to fix it.",
                    moreInfo = "https://restapi/error/API12345/moreinfo"
                });
            }
        }
    }
}