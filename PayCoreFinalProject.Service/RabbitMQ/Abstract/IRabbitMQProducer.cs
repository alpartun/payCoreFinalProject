using PayCoreFinalProject.Data.Model;

namespace PayCoreFinalProject.Service.RabbitMQ.Abstract;

public interface IRabbitMQProducer
{
    public void SendEmail(Email email);
}

