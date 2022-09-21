using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NHibernate;
using PayCoreFinalProject.Data.Model;
using PayCoreFinalProject.Data.Repository;
using PayCoreFinalProject.Service.RabbitMQ.Abstract;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PayCoreFinalProject.Service.RabbitMQ.Concrete;

public class RabbitMQConsumer : IRabbitMQConsumer
{
    protected readonly EmailSettings _emailSettings;
    protected readonly ISession _session;
    protected readonly IHibernateRepository<Email> _emailHibernateRepository;
    protected readonly RabbitMqSettings _rabbitMqSettings;
    public RabbitMQConsumer(IOptionsMonitor<EmailSettings> emailSettings,ISession session, IOptionsMonitor<RabbitMqSettings> rabbitMqSettings)
    {
        _session = session;
        _emailSettings = emailSettings.CurrentValue;
        _rabbitMqSettings = rabbitMqSettings.CurrentValue;
        _emailHibernateRepository = new HibernateRepository<Email>(session);
    }
    public Task Consume()
    {
        var con = new EmailSettings
        {
            User = _emailSettings.User,
            From = _emailSettings.From,
            Pass = _emailSettings.Pass,
            Port =  _emailSettings.Port,
            RetryCount = _emailSettings.RetryCount,
        };
        var factory = new ConnectionFactory
        {
            HostName = _rabbitMqSettings.HostName,
            UserName = _rabbitMqSettings.UserName,
            Password = _rabbitMqSettings.Password,
        };

        var connection = factory.CreateConnection();

         var channel = connection.CreateModel();

        channel.QueueDeclare("EmailQueue", exclusive: false);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += async (model, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" Message received: {message}");

            var text = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
            var result = JsonConvert.DeserializeObject<Email>(text);
            var mailSender = new EmailService.Concrete.EmailService(con);
            var email = new Email
            {
                EmailAdress = result.EmailAdress,
                EmailMessage = result.EmailMessage,
                EmailTitle = result.EmailTitle,
                IsSent = result.IsSent

            };
            await mailSender.SendEmail(email);
        };
        channel.BasicConsume(queue: "EmailQueue", autoAck: true, consumer: consumer);
        
        return Task.CompletedTask;
    }
}