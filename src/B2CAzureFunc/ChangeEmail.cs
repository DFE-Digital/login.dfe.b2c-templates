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
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;

namespace B2CAzureFunc
{
    /// <summary>
    /// ChangeEmail
    /// </summary>
    public class ChangeEmail
    {

        private readonly AppSettings _appSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appSettings"></param>
        public ChangeEmail(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings?.Value;
        }

        /// <summary>
        ///     ChangeEmail
        /// </summary>
        /// <verb>POST</verb>
        /// <url>http://localhost:7070/ChangeEmail</url>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <response code="200"><see cref="bool"/>User Found Response</response>
        /// <response code="400"><see cref="ResponseContentModel"/>Not Found</response>
        [FunctionName("ChangeEmail")]
        public async Task<IActionResult> Run(
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

                string tenant = _appSettings.B2CTenantId;// Environment.GetEnvironmentVariable("B2CTenantId", EnvironmentVariableTarget.Process);
                string clientId = _appSettings.B2CGraphAccessClientId.ToString();// Environment.GetEnvironmentVariable("B2CGraphAccessClientId", EnvironmentVariableTarget.Process);
                string clientSecret = _appSettings.B2CGraphAccessClientSecret;// Environment.GetEnvironmentVariable("B2CGraphAccessClientSecret", EnvironmentVariableTarget.Process);
                B2CGraphClient client = new B2CGraphClient(clientId, clientSecret, tenant);

                var newUser = await client.GetAllUsersAsync("$filter=signInNames/any(x:x/value eq '" + HttpUtility.UrlEncode(data.NewEmail) + "')");
                UserDetailsModel newUserDetails = JsonConvert.DeserializeObject<UserDetailsModel>(newUser);

                if (newUserDetails.value.Count > 0)
                {
                    return new BadRequestObjectResult(new ResponseContentModel
                    {
                        userMessage = "Sorry, This email already exists",
                    });
                }

                var currentUser = await client.GetUserByObjectId(data.ObjectId);
                if (!String.IsNullOrEmpty(currentUser))
                {
                    UserValueModel user = JsonConvert.DeserializeObject<UserValueModel>(currentUser);
                    log.LogInformation(currentUser);

                    if (user == null)
                    {
                        return new BadRequestObjectResult(new ResponseContentModel
                        {
                            userMessage = "Sorry, This user doesn't exists.",
                        });
                    }

                    bool updateResult = false;

                    if (!data.IsResend)
                    {
                        var extensionAppId = _appSettings.ExtensionAppId;// Environment.GetEnvironmentVariable("ExtensionAppId", EnvironmentVariableTarget.Process);
                        string json = "{\"extension_" + extensionAppId + "_IsEmailChangeRequested\":\"true\",\"extension_" + extensionAppId + "_NewEmail\":\"" + data.NewEmail + "\"}";
                        try
                        {
                            updateResult = await client.UpdateUser(data.ObjectId, json);
                        }
                        catch (Exception ex)
                        {
                            return new BadRequestObjectResult(new ResponseContentModel
                            {
                                userMessage = "Sorry, something happened unexpectedly while updating AD user.",
                            });
                        }
                    }

                    if (updateResult || data.IsResend)
                    {

                        var accountActivationEmailExpiryInSeconds = _appSettings.AccountActivationEmailExpiryInSeconds;// Convert.ToInt32(Environment.GetEnvironmentVariable("AccountActivationEmailExpiryInSeconds", EnvironmentVariableTarget.Process));


                        string token = TokenBuilder.BuildIdToken(user.signInNames.FirstOrDefault().value, data.NewEmail, DateTime.UtcNow.AddSeconds(accountActivationEmailExpiryInSeconds), req.Scheme, req.Host.Value, req.PathBase.Value, data.ObjectId, "changeemail",_appSettings.ClientSigningKey, _appSettings.RelyingPartyAppClientId.ToString());

                        string b2cURL = _appSettings.B2CAuthorizationUrl;// Environment.GetEnvironmentVariable("B2CAuthorizationUrl", EnvironmentVariableTarget.Process);
                        string b2cTenant = _appSettings.B2CTenant;// Environment.GetEnvironmentVariable("B2CTenant", EnvironmentVariableTarget.Process);
                        string b2cPolicyId = _appSettings.B2CChangeEmailPolicy;// Environment.GetEnvironmentVariable("B2CChangeEmailPolicy", EnvironmentVariableTarget.Process);
                        string b2cClientId = _appSettings.RelyingPartyAppClientId.ToString();// Environment.GetEnvironmentVariable("RelyingPartyAppClientId", EnvironmentVariableTarget.Process);
                        string b2cRedirectUri = _appSettings.B2CRedirectUri.ToString();// Environment.GetEnvironmentVariable("B2CRedirectUri", EnvironmentVariableTarget.Process);
                        string url = UrlBuilder.BuildUrl(token, b2cURL, b2cTenant, b2cPolicyId, b2cClientId, b2cRedirectUri);

                        string htmlTemplateOldEmail = _appSettings.NotifyEmailChangeConfirmationEmailOldEmailTemplateId.ToString();// Environment.GetEnvironmentVariable("NotifyEmailChangeConfirmationEmailOldEmailTemplateId", EnvironmentVariableTarget.Process);
                        string htmlTemplateNewEmail = _appSettings.NotifyEmailChangeConfirmationEmailNewEmailTemplateId.ToString();//Environment.GetEnvironmentVariable("NotifyEmailChangeConfirmationEmailNewEmailTemplateId", EnvironmentVariableTarget.Process);

                        bool result2 = false;
                        EmailModel model = new EmailModel
                        {
                            EmailTemplate = htmlTemplateNewEmail,
                            To = data.NewEmail.ToString(),
                            Personalisation = new Dictionary<string, dynamic>
                                            { {"name", user.givenName},
                                              {"link", url}
                                            }
                        };

                        var result1 = EmailService.Send(model);

                        if (!data.IsResend)
                        {
                            model = new EmailModel
                            {
                                EmailTemplate = htmlTemplateOldEmail,
                                To = user.signInNames.FirstOrDefault().value,
                                Personalisation = new Dictionary<string, dynamic>
                                            { {"name", user.givenName}
                                            }
                            };

                            result2 = EmailService.Send(model);
                        }
                        else
                        {
                            result2 = true;
                        }

                        if (result1 && result2 & data.SendTokenBackRequired)
                        {
                            return (ActionResult)new OkObjectResult(new { id_token_hint = token });
                        }

                        return result1 && result2
                            ? (ActionResult)new OkObjectResult(true)
                            : new BadRequestObjectResult(new ResponseContentModel
                            {
                                userMessage = "Failed to sent email, please contact support."
                            });
                    }
                    else
                    {
                        return new BadRequestObjectResult(new ResponseContentModel
                        {
                            userMessage = "Sorry, Something happened unexpectedly. Please try after sometime."
                        });
                    }
                }
                else
                {
                    return new BadRequestObjectResult(new ResponseContentModel
                    {
                        userMessage = "Sorry, This user doesn't exists.",
                    });
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new ResponseContentModel
                {
                    developerMessage = ex.ToString(),
                    userMessage = "Sorry, Something happened unexpectedly. Please try after sometime."
                });
            }
        }
    }
}