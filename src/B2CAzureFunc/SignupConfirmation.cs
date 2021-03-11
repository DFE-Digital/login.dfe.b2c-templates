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
    ///     SignupConfirmation
    /// </summary>
    public class SignupConfirmation
    {

        private readonly AppSettings _appSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appSettings"></param>
        public SignupConfirmation(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings?.Value;
        }

        /// <summary>
        ///     SignupConfirmation
        /// </summary>
        /// <verb>POST</verb>
        /// <url>http://localhost:7070/SignupConfirmation</url>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <response code="200"><see cref="bool"/>Signup Confirmed</response>
        /// <response code="409"><see cref="Object"/>Error</response>
        [FunctionName("SignupConfirmation")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("Request started");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<SignupConfirmationModel>(requestBody);
                log.LogInformation(requestBody);
                var accountActivationEmailExpiryInSeconds = _appSettings.AccountActivationEmailExpiryInSeconds;// Convert.ToInt32(Environment.GetEnvironmentVariable("AccountActivationEmailExpiryInSeconds", EnvironmentVariableTarget.Process));


                string token = TokenBuilder.BuildIdToken(data.Email.ToString(), DateTime.UtcNow.AddSeconds(accountActivationEmailExpiryInSeconds), req.Scheme, req.Host.Value, req.PathBase.Value, data.ObjectId.ToString(), data.GivenName, "selfsignup", _appSettings.ClientSigningKey, _appSettings.RelyingPartyAppClientId.ToString());

                string b2cURL = _appSettings.B2CAuthorizationUrl;// Environment.GetEnvironmentVariable("B2CAuthorizationUrl", EnvironmentVariableTarget.Process);
                string b2cTenant = _appSettings.B2CTenant;//Environment.GetEnvironmentVariable("B2CTenant", EnvironmentVariableTarget.Process);
                string b2cPolicyId = _appSettings.B2CSignUpPolicy;//Environment.GetEnvironmentVariable("B2CSignUpPolicy", EnvironmentVariableTarget.Process);
                string b2cClientId = _appSettings.RelyingPartyAppClientId.ToString();//Environment.GetEnvironmentVariable("RelyingPartyAppClientId", EnvironmentVariableTarget.Process);
                string b2cRedirectUri = _appSettings.B2CRedirectUri.ToString();//Environment.GetEnvironmentVariable("B2CRedirectUri", EnvironmentVariableTarget.Process);

                string url = UrlBuilder.BuildUrl(token, b2cURL, b2cTenant, b2cPolicyId, b2cClientId, b2cRedirectUri);

                string htmlTemplate = _appSettings.NotifyAidedSignupEmailTemplateId.ToString();// Environment.GetEnvironmentVariable("NotifySelfSignupEmailTemplateId", EnvironmentVariableTarget.Process);

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
                        userMessage = "Sorry, Something happened unexpectedly.",
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
                log.LogError(ex.ToString());

                return new BadRequestObjectResult(new ResponseContentModel
                {
                    userMessage = "Sorry, Something happened unexpectedly.",
                    version = "1.0.0",
                    status = 400,
                    code = "API12345",
                    requestId = "50f0bd91-2ff4-4b8f-828f-00f170519ddb",
                    developerMessage = "See logging provider failure dependencies for exception information.",
                    moreInfo = "https://restapi/error/API12345/moreinfo"
                });
            }
        }
    }
}
