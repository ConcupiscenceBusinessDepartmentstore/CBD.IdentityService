namespace CBD.IdentityService.WebAPI.Config;

public interface IMessageProducer
{
    void SendMessage<T>(T message);
}