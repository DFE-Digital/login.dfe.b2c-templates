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
using Microsoft.OpenApi.Extensions;
using Microsoft.Extensions.Options;

namespace B2CAzureFunc
{
    /// <summary>
    ///     SignUpInvitation
    /// </summary>
    public class SignupInvitation
    {

        private readonly AppSettings _appSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appSettings"></param>
        public SignupInvitation(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings?.Value;
        }

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
        public async Task<IActionResult> Run(
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
                    var getApiUrl = _appSettings.NcsDssGetCustomerApiUrl;
                    var dssApiUrl = String.Format(getApiUrl, data.CustomerId);

                    log.LogInformation(getApiUrl);

                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), dssApiUrl))
                    {
                        request.Headers.TryAddWithoutValidation("api-key", _appSettings.NcsDssApiKey);
                        request.Headers.TryAddWithoutValidation("version", _appSettings.NcsDssCustomersApiVersion);
                        request.Headers.TryAddWithoutValidation("Ocp-Apim-Subscription-Key", _appSettings.OcpApimSubscriptionKey);
                        request.Headers.TryAddWithoutValidation("TouchpointId", _appSettings.TouchpointId.ToString());

                        var response = await httpClient.SendAsync(request);
                        log.LogInformation(response.StatusCode.GetDisplayName() + " - " + response.StatusCode.ToString());

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var customer = JsonConvert.DeserializeObject<CustomerModel>(content);

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

                                var accountActivationEmailExpiryInSeconds = Convert.ToInt32(_appSettings.AccountActivationEmailExpiryInSeconds);//Environment.GetEnvironmentVariable("AccountActivationEmailExpiryInSeconds", EnvironmentVariableTarget.Process));

                                string token = TokenBuilder.BuildIdToken(data.Email.ToString(), data.GivenName.ToString(), data.LastName.ToString(), data.CustomerId.ToString(), DateTime.UtcNow.AddSeconds(accountActivationEmailExpiryInSeconds), req.Scheme, req.Host.Value, req.PathBase.Value, "aidedsignup", _appSettings.ClientSigningKey, _appSettings.RelyingPartyAppClientId.ToString());
                                string b2cURL = _appSettings.B2CAuthorizationUrl;// Environment.GetEnvironmentVariable("B2CAuthorizationUrl", EnvironmentVariableTarget.Process);
                                string b2cTenant = _appSettings.B2CTenant;//Environment.GetEnvironmentVariable("B2CTenant", EnvironmentVariableTarget.Process);
                                string b2cPolicyId = _appSettings.B2CSignUpPolicy;//Environment.GetEnvironmentVariable("B2CSignUpPolicy", EnvironmentVariableTarget.Process);
                                string b2cClientId = _appSettings.RelyingPartyAppClientId.ToString();//Environment.GetEnvironmentVariable("RelyingPartyAppClientId", EnvironmentVariableTarget.Process);
                                string b2cRedirectUri = _appSettings.B2CRedirectUri.ToString();//Environment.GetEnvironmentVariable("B2CRedirectUri", EnvironmentVariableTarget.Process);
                                string url = UrlBuilder.BuildUrl(token, b2cURL, b2cTenant, b2cPolicyId, b2cClientId, b2cRedirectUri);

                                string htmlTemplate = _appSettings.NotifyAidedSignupEmailTemplateId.ToString(); //Environment.GetEnvironmentVariable("NotifyAidedSignupEmailTemplateId", EnvironmentVariableTarget.Process);

                                EmailModel model = new EmailModel
                                {
                                    EmailTemplate = htmlTemplate,
                                    To = data.Email.ToString(),
                                    Personalisation = new Dictionary<string, dynamic>
                                            { {"name", data.GivenName.ToString()},
                                              {"link", url}
                                            }
                                };

                                var result = EmailService.Send(_appSettings.NotifyApiKey, model);
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
                            log.LogInformation(dssApiUrl);
                            return new BadRequestObjectResult(new ResponseContentModel
                            {
                                userMessage = "Failed to fetch customer details, please contact support",
                                developerMessage = "Apikey: " + _appSettings.NcsDssApiKey + " TouchPointId" + _appSettings.TouchpointId.ToString() + " NcsDssSearchApiVersion:" + _appSettings.NcsDssCustomersApiVersion
                                + " OcpApimSubscriptionKey:" + _appSettings.OcpApimSubscriptionKey
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                log.LogInformation(ex.ToString());
                return new BadRequestObjectResult(new ResponseContentModel
                {
                    userMessage = "Sorry, Something happened unexpectedly. Please try after sometime.",
                    developerMessage = "See logging provider failure dependencies for exception information."
                });
            }
        }
    }
}