using RabbitMQ.Client;

namespace BookStore.NotificationService.Messaging;

public sealed class RabbitMqConnection
{
    public IConnection Connection { get; }

    public RabbitMqConnection(IConnection connection)
    {
        Connection=connection;
    }
}
