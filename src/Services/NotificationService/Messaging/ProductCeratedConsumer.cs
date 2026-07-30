using System.Text;
using System.Text.Json;
using BookStore.NotificationService.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

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

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, args) =>
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var integrationEvent = JsonSerializer.Deserialize<ProductCreatedIntegrationEvent>(message);

            if(integrationEvent is null)
            {
                return;
            }


            Console.WriteLine("========== Product Created ==========");
            Console.WriteLine($"Id         : {integrationEvent.ProductId}");
            Console.WriteLine($"Name       : {integrationEvent.Name}");
            Console.WriteLine($"Price      : {integrationEvent.Price}");
            Console.WriteLine($"Currency   : {integrationEvent.Currency}");
            Console.WriteLine("=====================================");

            await channel.BasicAckAsync(deliveryTag: args.DeliveryTag, multiple: false, cancellationToken: cancellationToken);
        };

        await channel.BasicConsumeAsync(
            queue: "notification.product.created",
            autoAck: false,
            consumer: consumer,
            cancellationToken: cancellationToken);

        await Task.Delay(Timeout.Infinite, cancellationToken);

    }
}
