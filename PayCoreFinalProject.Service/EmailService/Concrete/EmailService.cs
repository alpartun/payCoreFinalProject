using AutoMapper;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using PayCoreFinalProject.Data.Model;

using PayCoreFinalProject.Service.EmailService.Abstract;


namespace PayCoreFinalProject.Service.EmailService.Concrete;

public class EmailService : IEmailService
{
    protected readonly EmailSettings _emailSettings;
    public EmailService(EmailSettings options)
    {

    }

    public EmailService(IOptionsMonitor<EmailSettings> optionsMonitor)
    {
        _emailSettings = optionsMonitor.CurrentValue;

    }
    public async Task SendEmail(Email email)
    {
        
        var emailTransform = new MimeMessage();
        emailTransform.From.Add(MailboxAddress.Parse("abel.kilback@ethereal.email"));
        emailTransform.To.Add(MailboxAddress.Parse(email.EmailAdress));
        emailTransform.Subject = email.EmailTitle;
        emailTransform.Body = new TextPart(TextFormat.Html) { Text = email.EmailMessage };
        // send email
        using (var smtp = new SmtpClient())
        {
            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("abel.kilback@ethereal.email", "qt3WdwZzRE2yeR9tsV");
            await smtp.SendAsync(emailTransform);
        }
    }
}

public class EmailNotSendException : Exception
{
}

