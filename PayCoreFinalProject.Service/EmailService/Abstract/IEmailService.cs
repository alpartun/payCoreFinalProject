using NHibernate;
using PayCoreFinalProject.Data.Model;
using PayCoreFinalProject.Data.Repository;


namespace PayCoreFinalProject.Service.EmailService.Abstract;

public interface IEmailService
{
    Task SendEmail(Email email);
    void Save(Email email);
}