using Microsoft.Extensions.Options;
using MimeKit;
using MVC.DAL.Models;
using MailKit.Net.Smtp;


namespace MVC.PL.Helpers.SendEmail
{
    public class MailService : IMailService
    {
        private MailSettings _options;

        public MailService(IOptions<MailSettings> options)
        {
            _options = options.Value;
        }
        public void SendEmail(Email email)
        {
            var mail = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_options.Email),
                Subject = email.Subject
            };
            mail.To.Add(MailboxAddress.Parse(email.To));
            var builder = new BodyBuilder();
            builder.TextBody = email.Body;
            mail.Body = builder.ToMessageBody();
            mail.From.Add(new MailboxAddress(_options.DisplayName, _options.Email));
            using var smtp = new SmtpClient();
            smtp.Connect(_options.Host, _options.Port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Email, _options.Password);
            smtp.Send(mail);
            smtp.Disconnect(true);
        }
    }
}
