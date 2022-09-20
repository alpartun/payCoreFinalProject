using System.Text;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using PayCoreFinalProject.Data.Model;
using PayCoreFinalProject.Service.RabbitMQ.Abstract;
using RabbitMQ.Client;

namespace PayCoreFinalProject.Service.RabbitMQ.Concrete;

public class RabbitMQProducer : IRabbitMQProducer
{
    public void SendEmail(Email email)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        var connectioon = factory.CreateConnection();

        using var channel = connectioon.CreateModel();

        channel.QueueDeclare("EmailQueue", exclusive: false);

        var json = JsonConvert.SerializeObject(email);
        var body = Encoding.UTF8.GetBytes(json);
        
        channel.BasicPublish(
            exchange:"",
            routingKey:"EmailQueue",
            body:body);

    }
}