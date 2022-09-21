using PayCoreFinalProject.Data.Model;

namespace PayCoreFinalProject.Service.RabbitMQ.Abstract;

public interface IRabbitMQProducer
{
    public Task Produce(Email email);
}