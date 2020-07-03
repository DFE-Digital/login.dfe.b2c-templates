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

namespace B2CAzureFunc
{
    /// <summary>
    ///     PasswordResetConfirmation
    /// </summary>
    public static class PasswordResetConfirmation
    {
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
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("Request started");


                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                log.LogInformation(requestBody);
                var accountActivationEmailExpiryInSeconds = Convert.ToInt32(Environment.GetEnvironmentVariable("AccountActivationEmailExpiryInSeconds", EnvironmentVariableTarget.Process));


                string token = TokenBuilder.BuildIdToken(data.email.ToString(), DateTime.UtcNow.AddSeconds(accountActivationEmailExpiryInSeconds), req.Scheme, req.Host.Value, req.PathBase.Value, data.ObjectId.ToString());

                string b2cURL = Environment.GetEnvironmentVariable("B2CAuthorizationUrl", EnvironmentVariableTarget.Process);
                string b2cTenant = Environment.GetEnvironmentVariable("B2CTenant", EnvironmentVariableTarget.Process);
                string b2cPolicyId = Environment.GetEnvironmentVariable("B2CPasswordResetConfirmPolicy", EnvironmentVariableTarget.Process);
                string b2cClientId = Environment.GetEnvironmentVariable("RelyingPartyAppClientId", EnvironmentVariableTarget.Process);
                string b2cRedirectUri = Environment.GetEnvironmentVariable("B2CRedirectUri", EnvironmentVariableTarget.Process);
                string url = UrlBuilder.BuildUrl(token, b2cURL, b2cTenant, b2cPolicyId, b2cClientId, b2cRedirectUri);

                string htmlTemplate = System.IO.File.ReadAllText(@"D:\home\site\wwwroot\EmailTemplates\ResetPassword\ResetPassword_inlined_css.html");
                string from = Environment.GetEnvironmentVariable("SMTPFromAddress", EnvironmentVariableTarget.Process);
                string subject = Environment.GetEnvironmentVariable("PasswordResetConfirmationEmailSubject", EnvironmentVariableTarget.Process);
                string fromDisplayName = Environment.GetEnvironmentVariable("FromDisplayName", EnvironmentVariableTarget.Process);
                htmlTemplate = htmlTemplate.Replace("#name#", data.givenName.ToString()).Replace("#link#", url);

                EmailModel model = new EmailModel
                {
                    Content = url,
                    EmailTemplate = htmlTemplate,
                    From = from,
                    Subject = subject,
                    To = data.email.ToString(),
                    Name = data.givenName.ToString(),
                    FromDisplayName = fromDisplayName
                };

                var result = EmailService.SendEmail(model);
                return result
                    ? (ActionResult)new OkObjectResult(true)
                    : new BadRequestObjectResult(new ResponseContentModel
                    {
                        userMessage = "Something happened unexpectedly.",
                        version = "1.0.0",
                        status = 409,
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
                    status = 409,
                    code = "API12345",
                    requestId = "50f0bd91-2ff4-4b8f-828f-00f170519ddb",
                    developerMessage = ex.ToString(),
                    moreInfo = "https://restapi/error/API12345/moreinfo"
                });
            }
        }
    }
}