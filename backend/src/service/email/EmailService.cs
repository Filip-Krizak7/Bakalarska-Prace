using TeacherPractise.Config;

using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace TeacherPractise.Service.Email
{
    public class EmailService : EmailSender
    {
        private readonly ILogger<EmailService> logger;

        public EmailService(ILogger<EmailService> logger)
        {
            this.logger = logger;
        }

        public async Task send(string to, string email)
        {
            try
            {
                MimeMessage mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress("", AppConfig.CONFIRMATION_EMAIL_ADDRESS));
                mimeMessage.To.Add(new MailboxAddress("", to));
                mimeMessage.Subject = "Potvrďte svou registraci pro registrační systém učitelských praxí";
                mimeMessage.Body = new TextPart("html")
                {
                    Text = email
                };
                
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                    await client.AuthenticateAsync(AppConfig.CONFIRMATION_EMAIL_ADDRESS, AppConfig.CONFIRMATION_EMAIL_PASSWORD);

                    await client.SendAsync(mimeMessage);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "failed to send email");
                throw new InvalidOperationException("failed to send email");
            }
        }

        public async Task sendForgotPasswordMail(string to, string email)
        {
            try
            {
                MimeMessage mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress("", AppConfig.CONFIRMATION_EMAIL_ADDRESS));
                mimeMessage.To.Add(new MailboxAddress("", to));
                mimeMessage.Subject = "Zapomenuté heslo - registrační systém učitelských praxí";
                mimeMessage.Body = new TextPart("html")
                {
                    Text = email
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                    await client.AuthenticateAsync(AppConfig.CONFIRMATION_EMAIL_ADDRESS, AppConfig.CONFIRMATION_EMAIL_PASSWORD);

                    await client.SendAsync(mimeMessage);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "failed to send email");
                throw new InvalidOperationException("failed to send email");
            }
        }
    }
}