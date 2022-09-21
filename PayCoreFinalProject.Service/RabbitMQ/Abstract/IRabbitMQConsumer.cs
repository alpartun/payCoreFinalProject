namespace PayCoreFinalProject.Service.RabbitMQ.Abstract;

public interface IRabbitMQConsumer
{
    public Task Consume();
}