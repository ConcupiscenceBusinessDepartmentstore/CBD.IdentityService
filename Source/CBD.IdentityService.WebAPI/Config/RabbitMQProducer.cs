using RabbitMQ.Client;

namespace CBD.IdentityService.WebAPI.Config;

public class RabbitMQProducer : IMessageProducer
{
    public void SendMessage<T>(T message)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
    }
}