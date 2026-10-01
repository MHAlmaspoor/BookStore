using System.Diagnostics;
using System.Text.Json;
using BookStore.BuildingBlocks.Messaging;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using RabbitMQ.Client;

namespace BookStore.BuildingBlocks.Infrastructure.Messaging;

public sealed class RabbitMqEventBus : IEventBus
{
    private readonly IRabbitMqConnection _connection;
    private static readonly TextMapPropagator propagator = Propagators.DefaultTextMapPropagator;
    public RabbitMqEventBus(IRabbitMqConnection connection)
    {
        _connection = connection;
    }
    public async Task PublishAsync(IntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        await using var channel = await _connection.Connection.CreateChannelAsync(cancellationToken: cancellationToken);

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

        propagator.Inject(new PropagationContext(Activity.Current?.Context ?? default, Baggage.Current),properties,
            static(carrier, key, value) =>
            {
                carrier.Headers ??= new Dictionary<string, object?>();
                carrier.Headers[key] = value;
            });

        await channel.BasicPublishAsync(
            exchange: RabbitMqConstants.ExchangeName,
            routingKey: integrationEvent.RoutingKey,
            mandatory: false,
            basicProperties: properties,
            body: body,
            cancellationToken: cancellationToken);
    }
}
