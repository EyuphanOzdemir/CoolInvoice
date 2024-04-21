using System.Net.Mail;
using Microsoft.Extensions.Logging;

namespace InvoicrInfrastructure.Email
{
  public class EmailSender(EmailConfig config) : IEmailSender
  {
        public async Task SendEmailAsync(string to, string from, string subject, string body)
        {

            // Create an instance of the SmtpClient
            using var smtpClient = new SmtpClient(config.SMTPServer, config.SMTPPort);
            // Create a MailMessage object
            using (var mailMessage = new MailMessage())
            {
                // Set the sender email address
                mailMessage.From = new MailAddress(from);

                // Set the recipient email address
                mailMessage.To.Add(new MailAddress(to));

                // Set the subject and body of the email
                mailMessage.Subject = subject;
                mailMessage.Body = body;

                // Send the email
                await smtpClient.SendMailAsync(mailMessage);
            }
        }

	}

}

