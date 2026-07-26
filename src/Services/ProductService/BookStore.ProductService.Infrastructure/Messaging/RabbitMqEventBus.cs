using System.Text;
using System.Text.Json;
using BookStore.ProductService.Application.Abstraction.Messaging;
using RabbitMQ.Client;

namespace BookStore.ProductService.Infrastructure.Messaging;

public sealed class RabbitMqEventBus:IEventBus
{
    private readonly IRabbitMqConnection _connection;
    public RabbitMqEventBus(IRabbitMqConnection connection)
    {
        _connection=connection;
    }

    public async Task PublishAsync(IntegrationEvent integrationEvent, CancellationToken cancellationToken=default)
    {
        var channel =await _connection.Connection.CreateChannelAsync(cancellationToken: cancellationToken);
        await channel.ExchangeDeclareAsync(
            exchange: "Bookstore.events",
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false,
            cancellationToken: cancellationToken );

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(integrationEvent));

        var properties = new BasicProperties
        {
            Persistent = true,
            ContentType = "application/json"
        };

        const string routingKey = "product.created";

        await channel.BasicPublishAsync(
            exchange: "bookstore.events",
            routingKey: routingKey,
            mandatory: false,
            basicProperties: properties,
            body: body,
            cancellationToken: cancellationToken);
        
    }
}
