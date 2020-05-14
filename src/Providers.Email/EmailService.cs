using Providers.Email.Model;
using System;
using System.Net;
using System.Net.Mail;

namespace Providers.Email
{
    public class EmailService
    {
        public static bool SendEmail(EmailModel model)
        {
                MailMessage mailMessage = new MailMessage();
                mailMessage.To.Add(model.To);
                mailMessage.From = new MailAddress(Environment.GetEnvironmentVariable("SMTPFromAddress", EnvironmentVariableTarget.Process));
                mailMessage.Subject = model.Subject;
                mailMessage.Body = string.Format(model.EmailTemplate, model.Name, model.Content);
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