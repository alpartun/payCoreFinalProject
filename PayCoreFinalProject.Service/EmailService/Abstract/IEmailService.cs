using PayCoreFinalProject.Data.Model;


namespace PayCoreFinalProject.Service.EmailService.Abstract;

public interface IEmailService 
{
    Task SendEmail(Email email);
    void SaveEmail(Email email);
}