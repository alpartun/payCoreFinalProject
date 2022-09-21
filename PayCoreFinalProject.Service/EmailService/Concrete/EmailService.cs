using AutoMapper;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using NHibernate;
using PayCoreFinalProject.Data.Model;

using PayCoreFinalProject.Service.EmailService.Abstract;


namespace PayCoreFinalProject.Service.EmailService.Concrete;

public class EmailService : IEmailService
{
    protected readonly EmailSettings _emailSettings;
    protected readonly ISession _session;
    public EmailService(IOptionsMonitor<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.CurrentValue;

    }
    public async Task SendEmail(Email email)
    {
        
        var emailTransform = new MimeMessage();
        emailTransform.From.Add(MailboxAddress.Parse(_emailSettings.From));
        emailTransform.To.Add(MailboxAddress.Parse(email.EmailAdress));
        emailTransform.Subject = email.EmailTitle;
        emailTransform.Body = new TextPart(TextFormat.Html) { Text = email.EmailMessage };
        // send email
        using (var smtp = new SmtpClient())
        {
            smtp.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailSettings.User, _emailSettings.Pass);
            await smtp.SendAsync(emailTransform);
        }
    }
}

public class EmailNotSendException : Exception
{
}

