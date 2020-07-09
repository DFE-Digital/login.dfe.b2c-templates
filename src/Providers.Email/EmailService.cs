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
        /// Send Email
        /// </summary>
        /// <param name="model"></param>
        /// <returns>bool</returns>
        public static bool SendEmail(EmailModel model)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.To.Add(model.To);
            mailMessage.From = new MailAddress(model.From, model.FromDisplayName);
            mailMessage.Subject = model.Subject;
            mailMessage.Body = model.EmailTemplate;
            mailMessage.IsBodyHtml = true;
            SmtpClient smtpClient = new SmtpClient(Environment.GetEnvironmentVariable("SMTPServer", EnvironmentVariableTarget.Process), Convert.ToInt32(Environment.GetEnvironmentVariable("SMTPPort", EnvironmentVariableTarget.Process)));
            smtpClient.Credentials = new NetworkCredential(Environment.GetEnvironmentVariable("SMTPUsername", EnvironmentVariableTarget.Process), Environment.GetEnvironmentVariable("SMTPPassword", EnvironmentVariableTarget.Process));
            smtpClient.EnableSsl = Convert.ToBoolean(Environment.GetEnvironmentVariable("SMTPUseSSL", EnvironmentVariableTarget.Process));
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Send(mailMessage);
            return true;
        }

        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="model"></param>
        /// <returns>bool</returns>
        //public static bool Send(EmailModel model)
        //{
        //    var client = new NotificationClient("emailintegration-f0833c0b-c57b-48c2-b455-62cf80226d42-89b7c7f6-8e59-4df6-ab62-27e7661e5a10");// (Environment.GetEnvironmentVariable("NotifyAPIKey", EnvironmentVariableTarget.Process));
        //    EmailNotificationResponse response = client.SendEmail(model.To, model.EmailTemplate, model.Personalisation);
        //    return true;
        //}
    }
}