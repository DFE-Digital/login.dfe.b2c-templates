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
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace B2CAzureFunc
{
    /// <summary>
    ///     PasswordResetConfirmation
    /// </summary>
    public class PasswordResetConfirmation
    {
        private readonly AppSettings _appSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appSettings"></param>
        public PasswordResetConfirmation(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings?.Value;
        }

        /// <summary>
        ///     PasswordResetConfirmation
        /// </summary>
        /// <verb>POST</verb>
        /// <url>http://localhost:7070/PasswordResetConfirmation</url>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <response code="200"><see cref="bool"/>Password Reset Sent</response>
        /// <response code="409"><see cref="Object"/>Error</response>
        [FunctionName("PasswordResetConfirmation")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("Request started");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                log.LogInformation(requestBody);
                var accountActivationEmailExpiryInSeconds = _appSettings.AccountActivationEmailExpiryInSeconds;// Convert.ToInt32(Environment.GetEnvironmentVariable("AccountActivationEmailExpiryInSeconds", EnvironmentVariableTarget.Process));


                string token = TokenBuilder.BuildIdToken(data.email.ToString(), DateTime.UtcNow.AddSeconds(accountActivationEmailExpiryInSeconds), req.Scheme, req.Host.Value, req.PathBase.Value, data.ObjectId.ToString(), data.givenName.ToString(), "passwordreset", _appSettings.ClientSigningKey, _appSettings.RelyingPartyAppClientId.ToString());

                string b2cURL = _appSettings.B2CAuthorizationUrl;// Environment.GetEnvironmentVariable("B2CAuthorizationUrl", EnvironmentVariableTarget.Process);
                string b2cTenant = _appSettings.B2CTenant;//Environment.GetEnvironmentVariable("B2CTenant", EnvironmentVariableTarget.Process);
                string b2cPolicyId = _appSettings.B2CPasswordResetConfirmPolicy;//Environment.GetEnvironmentVariable("B2CSignUpPolicy", EnvironmentVariableTarget.Process);
                string b2cClientId = _appSettings.RelyingPartyAppClientId.ToString();//Environment.GetEnvironmentVariable("RelyingPartyAppClientId", EnvironmentVariableTarget.Process);
                string b2cRedirectUri = _appSettings.B2CRedirectUri.ToString();//Environment.GetEnvironmentVariable("B2CRedirectUri", EnvironmentVariableTarget.Process);

                string url = UrlBuilder.BuildUrl(token, b2cURL, b2cTenant, b2cPolicyId, b2cClientId, b2cRedirectUri);

                string htmlTemplate = _appSettings.NotifyPasswordResetConfirmationEmailTemplateId.ToString();// Environment.GetEnvironmentVariable("NotifyPasswordResetConfirmationEmailTemplateId", EnvironmentVariableTarget.Process);


                EmailModel model = new EmailModel
                {
                    EmailTemplate = htmlTemplate,
                    To = data.email.ToString(),
                    Personalisation = new Dictionary<string, dynamic>
                                            { {"name", data.givenName.ToString()},
                                              {"link", url}
                                            }
                };

                var result = EmailService.Send(_appSettings.NotifyApiKey, model);
                return result
                    ? (ActionResult)new OkObjectResult(true)
                    : new BadRequestObjectResult(new ResponseContentModel
                    {
                        userMessage = "Something happened unexpectedly.",
                        version = "1.0.0",
                        status = 400,
                        code = "API12345",
                        requestId = "50f0bd91-2ff4-4b8f-828f-00f170519ddb",
                        developerMessage = "Email sent failed.",
                        moreInfo = "https://restapi/error/API12345/moreinfo"
                    });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new ResponseContentModel
                {
                    userMessage = ex.ToString(),
                    version = "1.0.0",
                    status = 400,
                    code = "API12345",
                    requestId = "50f0bd91-2ff4-4b8f-828f-00f170519ddb",
                    developerMessage = ex.ToString(),
                    moreInfo = "https://restapi/error/API12345/moreinfo"
                });
            }
        }
    }
}