using Providers.Email.Model;
using System;
using System.Net;
using System.Net.Mail;

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
    }
}