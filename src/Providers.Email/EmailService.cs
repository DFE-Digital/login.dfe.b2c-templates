using Providers.Email.Model;
using System;
using System.Net;
using System.Net.Mail;
using Notify.Client;
using Notify.Models;
using Notify.Models.Responses;

namespace Providers.Email
{
    /// <summary>
    ///     Email Service
    /// </summary>
    public class EmailService
    {
        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="model"></param>
        /// <returns>bool</returns>
        public static bool Send(EmailModel model)
        {
            var client = new NotificationClient(Environment.GetEnvironmentVariable("NotifyAPIKey", EnvironmentVariableTarget.Process)); //("emailintegration-f0833c0b-c57b-48c2-b455-62cf80226d42-89b7c7f6-8e59-4df6-ab62-27e7661e5a10");
            EmailNotificationResponse response = client.SendEmail(model.To, model.EmailTemplate, model.Personalisation);
            if (response != null && !String.IsNullOrEmpty(response.id))
                return true;
            else
                return false;
        }
    }
}