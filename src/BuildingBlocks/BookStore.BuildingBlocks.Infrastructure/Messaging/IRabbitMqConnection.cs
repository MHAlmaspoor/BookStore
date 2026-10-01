using RabbitMQ.Client;

namespace BookStore.BuildingBlocks.Infrastructure.Messaging;

public interface IRabbitMqConnection : IAsyncDisposable
{
    IConnection Connection { get; }
}
