/*// See https://aka.ms/new-console-template for more information

using System.Text;
using Newtonsoft.Json;
using PayCoreFinalProject.Data.Model;
using PayCoreFinalProject.Service.EmailService.Concrete;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace PayCoreFinalProject.RabbitMQ.Consumer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();            

            Consume(config);
        }

        static void Consume(IConfiguration config)
        {
            var con = new EmailSettings
            {
                User = config.GetSection("Email")["User"],
                From = config.GetSection("Email")["From"],
                Pass = config.GetSection("Email")["Pass"],
                Port = int.Parse(config.GetSection("Email")["Port"]),
                RetryCount = int.Parse(config.GetSection("Email")["RetryCount"])
            };
            var factory = new ConnectionFactory
            {
                HostName = config.GetSection("RabbitMQ")["HostName"],
                UserName = config.GetSection("RabbitMQ")["UserName"],
                Password = config.GetSection("RabbitMQ")["Password"]
            };

            var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();

            channel.QueueDeclare("EmailQueue", exclusive: false);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" Message received: {message}");

                var text = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                var result = JsonConvert.DeserializeObject<Email>(text);
                EmailService mailSender = new EmailService();
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
            Console.ReadKey();
        }


    }
}*/


