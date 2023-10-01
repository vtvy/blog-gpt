using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BlogGPT.Infrastructure.Services
{
    public class MailSettings
    {
        public string Mail { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }

    }

    public class MailService : IEmailSender
    {
        private readonly MailSettings mailSettings;

        private readonly ILogger<MailService> logger;

        public MailService(IOptions<MailSettings> _mailSettings, ILogger<MailService> _logger)
        {
            mailSettings = _mailSettings.Value;
            logger = _logger;
            logger.LogInformation("Create MailService");
        }


        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage
            {
                Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail),
                Subject = subject
            };
            message.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
            message.To.Add(MailboxAddress.Parse(email));


            var builder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };
            message.Body = builder.ToMessageBody();

            // Use MailKit SmtpClient
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
                await smtp.SendAsync(message);
            }
            catch (Exception ex)
            {
                logger.LogError($"Fail to send mail: {ex.Message}");
            }

            smtp.Disconnect(true);

            logger.LogInformation($"Send mail to {email} successfully");
        }
    }
}
