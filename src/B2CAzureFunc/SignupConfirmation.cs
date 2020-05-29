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
    ///     SignupConfirmation
    /// </summary>
    public static class SignupConfirmation
    {
        /// <summary>
        ///     SignupConfirmation
        /// </summary>
        /// <verb>POST</verb>
        /// <url>http://localhost:7070/api/SignupConfirmation</url>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <response code="200"><see cref="bool"/>Signup Confirmed</response>
        /// <response code="409"><see cref="Object"/>Error</response>
        [FunctionName("SignupConfirmation")]
        public static async Task<IActionResult> Run(
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
                var accountActivationEmailExpiryInSeconds = Convert.ToInt32(Environment.GetEnvironmentVariable("AccountActivationEmailExpiryInSeconds", EnvironmentVariableTarget.Process));


                string token = TokenBuilder.BuildIdToken(data.Email.ToString(), DateTime.UtcNow.AddSeconds(accountActivationEmailExpiryInSeconds), req.Scheme, req.Host.Value, req.PathBase.Value, data.ObjectId.ToString());

                string b2cURL = Environment.GetEnvironmentVariable("B2CAuthorizationUrl", EnvironmentVariableTarget.Process);
                string b2cTenant = Environment.GetEnvironmentVariable("B2CTenant", EnvironmentVariableTarget.Process);
                string b2cPolicyId = Environment.GetEnvironmentVariable("B2CSignupConirmPolicy", EnvironmentVariableTarget.Process);
                string b2cClientId = Environment.GetEnvironmentVariable("B2CClientId", EnvironmentVariableTarget.Process);
                string b2cRedirectUri = Environment.GetEnvironmentVariable("B2CRedirectUri", EnvironmentVariableTarget.Process);
                string url = UrlBuilder.BuildUrl(token, b2cURL, b2cTenant, b2cPolicyId, b2cClientId, b2cRedirectUri);

                string htmlTemplate = System.IO.File.ReadAllText(@"D:\home\site\wwwroot\EmailTemplates\Signup\Signup_inlined_css.html");
                string from = Environment.GetEnvironmentVariable("SMTPFromAddress", EnvironmentVariableTarget.Process);
                string subject = "";
                if (!data.IsResend)
                    subject = Environment.GetEnvironmentVariable("SignupConfirmationEmailSubject", EnvironmentVariableTarget.Process);
                else
                    subject = Environment.GetEnvironmentVariable("resendSignupEmail", EnvironmentVariableTarget.Process);

                htmlTemplate = htmlTemplate.Replace("#name#", data.GivenName.ToString()).Replace("#link#", url);

                EmailModel model = new EmailModel
                {
                    Content = url,
                    EmailTemplate = htmlTemplate,
                    From = from,
                    Subject = subject,
                    To = data.Email.ToString(),
                    Name = data.GivenName.ToString()
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
