using RabbitMQ.Client;

namespace BookStore.BuildingBlocks.Infrastructure.Messaging;

public class RabbitMqConnection : IRabbitMqConnection
{
    public IConnection Connection { get; }

    public RabbitMqConnection(IConnection connection)
    {
        Connection = connection;
    }

    public async ValueTask DisposeAsync()
    {
        await Connection.DisposeAsync();
    }
}
