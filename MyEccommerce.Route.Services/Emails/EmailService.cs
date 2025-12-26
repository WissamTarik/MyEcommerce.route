using Microsoft.Extensions.Options;
using MimeKit;
using MyEccommerce.Route.Services.Abstractions.Emails;
using MyEccommerce.Route.Shared.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace MyEccommerce.Route.Services.Emails
{
    public class EmailService(IOptions<EmailSettingsOptions> _options) : IEmailService
    {
        public  async  Task SendEmailAsync(string toEmail, string body, string subject)
        {
            var EmailSettings = _options.Value;
            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress(EmailSettings.SenderName, EmailSettings.SenderEmail));
            mail.To.Add(MailboxAddress.Parse(toEmail));
            mail.Subject=subject;

            var builder = new BodyBuilder() { HtmlBody=body};

            mail.Body = builder.ToMessageBody();
            await EstablishConnection(EmailSettings, mail);
        }
        private async Task EstablishConnection(EmailSettingsOptions? options,MimeMessage ? mail)
        {
            using var smtp = new SmtpClient();
           await  smtp.ConnectAsync(options.SmtpServer, options.Port,SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(options.Username, options.Password);
            await smtp.SendAsync(mail);
            await smtp.DisconnectAsync(true);
        }
    }
}
