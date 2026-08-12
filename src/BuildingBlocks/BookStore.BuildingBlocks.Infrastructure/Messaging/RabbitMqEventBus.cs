using System.Text.Json;
using BookStore.BuildingBlocks.Messaging;
using RabbitMQ.Client;

namespace BookStore.BuildingBlocks.Infrastructure.Messaging;

public sealed class RabbitMqEventBus : IEventBus
{
    private readonly IRabbitMqConnection _connection;

    public RabbitMqEventBus(IRabbitMqConnection connection)
    {
        _connection = connection;
    }
    public async Task PublishAsync(IntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        await using var channel = await _connection.Connection
            .CreateChannelAsync(cancellationToken: cancellationToken);

        await channel.ExchangeDeclareAsync(
            exchange: RabbitMqConstants.ExchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false,
            cancellationToken: cancellationToken);

        var body =JsonSerializer.SerializeToUtf8Bytes(integrationEvent,integrationEvent.GetType());

        var properties = new BasicProperties
        {
            Persistent = true,
            ContentType="application/json"
        };

        await channel.BasicPublishAsync(
            exchange: RabbitMqConstants.ExchangeName,
            routingKey: integrationEvent.RoutingKey,
            mandatory: false,
            basicProperties: properties,
            body: body,
            cancellationToken: cancellationToken);
    }
}
