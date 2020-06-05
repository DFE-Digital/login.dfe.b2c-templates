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
using B2CAzureFunc.Helpers;
using Providers.Email.Model;
using Providers.Email;
using System.Web;

namespace B2CAzureFunc
{
    /// <summary>
    /// ChangeEmail
    /// </summary>
    public static class ChangeEmail
    {
        [FunctionName("ChangeEmail")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                log.LogInformation("Request started");
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                ChangeEmailModel data = JsonConvert.DeserializeObject<ChangeEmailModel>(requestBody);
                log.LogInformation(requestBody);

                string tenant = Environment.GetEnvironmentVariable("b2c:Tenant", EnvironmentVariableTarget.Process);
                string clientId = Environment.GetEnvironmentVariable("b2c:GraphAccessClientId", EnvironmentVariableTarget.Process);
                string clientSecret = Environment.GetEnvironmentVariable("b2c:GraphAccessClientSecret", EnvironmentVariableTarget.Process);
                B2CGraphClient client = new B2CGraphClient(clientId, clientSecret, tenant);

                var newUser = await client.GetAllUsersAsync("$filter=signInNames/any(x:x/value eq '" + HttpUtility.UrlEncode(data.NewEmail) + "')");
                UserDetailsModel newUserDetails = JsonConvert.DeserializeObject<UserDetailsModel>(newUser);
                if (newUserDetails.value.Count > 0)
                {
                    return new BadRequestObjectResult(new ResponseContentModel
                    {
                        version = "1.0.0",
                        userMessage = "Sorry, This email already exists",
                        status = 404
                    });
                }

                var currentUser = await client.GetAllUsersAsync("$filter=signInNames/any(x:x/value eq '" + HttpUtility.UrlEncode(data.CurrentEmail) + "')");
                UserDetailsModel userDetails = JsonConvert.DeserializeObject<UserDetailsModel>(currentUser);
                log.LogInformation(currentUser);
                if (!(userDetails.value.Count > 0))
                {
                    return new BadRequestObjectResult(new ResponseContentModel
                    {
                        version = "1.0.0",
                        userMessage = "Sorry, This user doesn't exists.",
                        status = 404
                    });
                }
                bool updateResult = false;
                if (!data.IsResend)
                {
                    var extensionAppId = Environment.GetEnvironmentVariable("ExtensionAppId", EnvironmentVariableTarget.Process);
                    string json = "{\"extension_" + extensionAppId + "_IsEmailChangeRequested\":\"true\",\"extension_" + extensionAppId + "_NewEmail\":\"" + data.NewEmail + "\"}";

                    updateResult = await client.UpdateUser(userDetails.value[0].objectId, json);
                }

                if (updateResult || data.IsResend)
                {

                    var accountActivationEmailExpiryInSeconds = Convert.ToInt32(Environment.GetEnvironmentVariable("AccountActivationEmailExpiryInSeconds", EnvironmentVariableTarget.Process));


                    string token = TokenBuilder.BuildIdToken(data.CurrentEmail, data.NewEmail, DateTime.UtcNow.AddSeconds(accountActivationEmailExpiryInSeconds), req.Scheme, req.Host.Value, req.PathBase.Value, userDetails.value[0].objectId);

                    string b2cURL = Environment.GetEnvironmentVariable("B2CAuthorizationUrl", EnvironmentVariableTarget.Process);
                    string b2cTenant = Environment.GetEnvironmentVariable("B2CTenant", EnvironmentVariableTarget.Process);
                    string b2cPolicyId = Environment.GetEnvironmentVariable("B2CChangeEmailPolicy", EnvironmentVariableTarget.Process);
                    string b2cClientId = Environment.GetEnvironmentVariable("B2CClientId", EnvironmentVariableTarget.Process);
                    string b2cRedirectUri = Environment.GetEnvironmentVariable("B2CRedirectUri", EnvironmentVariableTarget.Process);
                    string url = UrlBuilder.BuildUrl(token, b2cURL, b2cTenant, b2cPolicyId, b2cClientId, b2cRedirectUri);

                    string htmlTemplateOldEmail = System.IO.File.ReadAllText(@"D:\home\site\wwwroot\EmailChnage_OldEmail.html");
                    string htmlTemplateNewEmail = System.IO.File.ReadAllText(@"D:\home\site\wwwroot\EmailChnage_NewEmail.html");

                    string from = Environment.GetEnvironmentVariable("SMTPFromAddress", EnvironmentVariableTarget.Process);
                    string emailChangeSubjectToNewEmail = Environment.GetEnvironmentVariable("EmailChangeConfirmationEmailSubjectNewEmail", EnvironmentVariableTarget.Process);
                    string emailChangeSubjectToOldEmail = Environment.GetEnvironmentVariable("EmailChangeConfirmationEmailSubjectOldEmail", EnvironmentVariableTarget.Process);

                    htmlTemplateOldEmail = htmlTemplateOldEmail.Replace("#name#", userDetails.value[0].givenName);
                    htmlTemplateNewEmail = htmlTemplateNewEmail.Replace("#name#", userDetails.value[0].givenName).Replace("#link#", url);
                    bool result2 = false;
                    EmailModel model = new EmailModel
                    {
                        Content = url,
                        EmailTemplate = htmlTemplateNewEmail,
                        From = from,
                        Subject = emailChangeSubjectToNewEmail,
                        To = data.NewEmail.ToString(),
                        Name = userDetails.value[0].givenName.ToString()
                    };

                    var result1 = EmailService.SendEmail(model);

                    if (!data.IsResend)
                    {

                        model = new EmailModel
                        {
                            Content = "",
                            EmailTemplate = htmlTemplateOldEmail,
                            From = from,
                            Subject = emailChangeSubjectToOldEmail,
                            To = data.CurrentEmail.ToString(),
                            Name = userDetails.value[0].givenName.ToString()
                        };

                        result2 = EmailService.SendEmail(model);
                    }
                    else
                        result2 = true;

                    return result1 && result2
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
                else
                    return new BadRequestObjectResult(new ResponseContentModel
                    {
                        version = "1.0.0",
                        userMessage = "Sorry, Something happened unexpectedly. Please try after sometime.",
                        status = 404
                    });

            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new ResponseContentModel
                {
                    version = "1.0.0",
                    userMessage = ex.ToString(),
                    status = 404
                });
            }
        }
    }
}