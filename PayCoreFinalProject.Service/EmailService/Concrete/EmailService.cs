using AutoMapper;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using NHibernate;
using PayCoreFinalProject.Data.Model;
using PayCoreFinalProject.Data.Repository;
using PayCoreFinalProject.Service.EmailService.Abstract;
using Polly;
using Serilog;


namespace PayCoreFinalProject.Service.EmailService.Concrete;

public class EmailService : IEmailService
{
    protected readonly EmailSettings _emailSettings;
    protected readonly ISession _session;
    protected readonly IHibernateRepository<Email> _emailHibernateRepository;

    public EmailService(IOptionsMonitor<EmailSettings> emailSettings, ISession session)
    {
        _session = session;
        _emailSettings = emailSettings.CurrentValue;
        _emailHibernateRepository = new HibernateRepository<Email>(session);
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
            email.SendTime = DateTime.Now;

            await smtp.SendAsync(emailTransform);
            email.IsSent = true;
        }

        var retryPolicy = Policy
            .Handle<EmailNotSendException>()
            .Retry(_emailSettings.RetryCount);
        retryPolicy.Execute((() =>
        {
            using (var smtp = new SmtpClient())
            {
                smtp.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailSettings.User, _emailSettings.Pass);
                email.SendTime = DateTime.Now;

                email.IsSent = smtp.SendAsync(emailTransform).IsCompletedSuccessfully;
            }
        }));
    }


    public void Save(Email email)
    {
        try
        {
            _emailHibernateRepository.BeginTransaction();
            _emailHibernateRepository.Save(email);
            _emailHibernateRepository.Commit();
            _emailHibernateRepository.CloseTransaction();
        }
        catch (Exception e)
        {
            _emailHibernateRepository.Rollback();
            _emailHibernateRepository.CloseTransaction();
            Log.Error("EmailService.Save", e);
        }
    }
}

public class EmailNotSendException : Exception
{
}