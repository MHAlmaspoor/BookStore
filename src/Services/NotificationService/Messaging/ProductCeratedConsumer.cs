using RabbitMQ.Client;

namespace BookStore.NotificationService.Messaging;

public sealed class ProductCreatedConsumer
{
    private readonly RabbitMqConnection _connection;
    public ProductCreatedConsumer(RabbitMqConnection connection)
    {
        _connection=connection;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var channel = await _connection.Connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            queue: "notification.product.created",
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await channel.ExchangeDeclareAsync(
            exchange: "bookstore.events",
            type: ExchangeType.Topic,
            durable: false,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await channel.QueueBindAsync(
            queue: "notification.product.created",
            exchange: "bookstore.events",
            routingKey: "product.created",
            cancellationToken: cancellationToken);

    }
}
