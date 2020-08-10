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
using System.Collections.Generic;

namespace B2CAzureFunc
{
    /// <summary>
    ///     SignUpInvitation
    /// </summary>
    public static class SignupInvitation
    {
        /// <summary>
        ///     SignUpInvitation
        /// </summary>
        /// <verb>POST</verb>
        /// <url>http://localhost:7070/SignUpInvitation</url>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <response code="200"><see cref="bool"/>Invitation Sent</response>
        /// <response code="404"><see cref="Object"/>Error</response>
        [FunctionName("SignupInvitation")]
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

                if (String.IsNullOrEmpty(data.CustomerId) || String.IsNullOrEmpty(data.Email) || String.IsNullOrEmpty(data.GivenName)
                    || String.IsNullOrEmpty(data.LastName))
                {
                    return new BadRequestObjectResult(new ResponseContentModel
                    {
                        userMessage = "Please check the input",
                    });
                }

                using (var httpClient = new HttpClient())
                {
                    var getApiUrl = Environment.GetEnvironmentVariable("ncs-dss-get-customer-api-url", EnvironmentVariableTarget.Process);
                    var dssApiUrl = String.Format(getApiUrl, data.CustomerId);
                    log.LogInformation(dssApiUrl);

                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), dssApiUrl))
                    {
                        request.Headers.TryAddWithoutValidation("api-key", Environment.GetEnvironmentVariable("ncs-dss-api-key", EnvironmentVariableTarget.Process));
                        request.Headers.TryAddWithoutValidation("version", Environment.GetEnvironmentVariable("ncs-dss-search-api-version", EnvironmentVariableTarget.Process));
                        request.Headers.TryAddWithoutValidation("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("Ocp-Apim-Subscription-Key", EnvironmentVariableTarget.Process));

                        var response = await httpClient.SendAsync(request);
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            log.LogInformation(result);
                            var customer = JsonConvert.DeserializeObject<CustomerModel>(result);
                            if (customer == null)
                            {
                                return new BadRequestObjectResult(new ResponseContentModel
                                {
                                    userMessage = "We have not been able to find your account"
                                });
                            }
                            else
                            {
                                if (data.GivenName.ToLower() == customer.GivenName.ToLower() && data.LastName.ToLower() == customer.FamilyName.ToLower())
                                    data.CustomerId = customer.CustomerId.ToString();
                                else
                                    return new BadRequestObjectResult(new ResponseContentModel
                                    {
                                        userMessage = "We have not been able to find your account"
                                    });

                                var accountActivationEmailExpiryInSeconds = Convert.ToInt32(Environment.GetEnvironmentVariable("AccountActivationEmailExpiryInSeconds", EnvironmentVariableTarget.Process));

                                string token = TokenBuilder.BuildIdToken(data.Email.ToString(), data.GivenName.ToString(), data.LastName.ToString(), data.CustomerId.ToString(), DateTime.UtcNow.AddSeconds(accountActivationEmailExpiryInSeconds), req.Scheme, req.Host.Value, req.PathBase.Value, "aidedsignup");
                                string b2cURL = Environment.GetEnvironmentVariable("B2CAuthorizationUrl", EnvironmentVariableTarget.Process);
                                string b2cTenant = Environment.GetEnvironmentVariable("B2CTenant", EnvironmentVariableTarget.Process);
                                string b2cPolicyId = Environment.GetEnvironmentVariable("B2CSignUpPolicy", EnvironmentVariableTarget.Process);
                                string b2cClientId = Environment.GetEnvironmentVariable("RelyingPartyAppClientId", EnvironmentVariableTarget.Process);
                                string b2cRedirectUri = Environment.GetEnvironmentVariable("B2CRedirectUri", EnvironmentVariableTarget.Process);
                                string url = UrlBuilder.BuildUrl(token, b2cURL, b2cTenant, b2cPolicyId, b2cClientId, b2cRedirectUri);

                                string htmlTemplate = Environment.GetEnvironmentVariable("NotifyAidedSignupEmailTemplateId", EnvironmentVariableTarget.Process);

                                EmailModel model = new EmailModel
                                {
                                    EmailTemplate = htmlTemplate,
                                    To = data.Email.ToString(),
                                    Personalisation = new Dictionary<string, dynamic>
                                            { {"name", data.GivenName.ToString()},
                                              {"link", url}
                                            }
                                };

                                var result = EmailService.Send(model);
                                return result
                                    ? (ActionResult)new OkObjectResult(true)
                                    : new BadRequestObjectResult(new ResponseContentModel
                                    {
                                        userMessage = "Couldn't send email to user."
                                    });
                            }
                        }
                        else
                        {
                            return new BadRequestObjectResult(new ResponseContentModel
                            {
                                userMessage = "Failed to fetch customer details, please contact support"
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogInformation(ex.ToString());
                return new BadRequestObjectResult(new ResponseContentModel
                {
                    userMessage = "Sorry, Something happened unexpectedly. Please try after sometime."
                });
            }
        }
    }
}