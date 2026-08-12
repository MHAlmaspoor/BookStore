using System.Collections;
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
            queue: "notification-service",
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await channel.ExchangeDeclareAsync(
            exchange: "bookstore.events",
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await channel.QueueBindAsync(
            queue: "notification-service",
            exchange: "bookstore.events",
            routingKey: "product.created",
            cancellationToken: cancellationToken);

        await channel.QueueBindAsync(
            queue: "notification-service",
            exchange: "bookstore.events",
            routingKey: "user.registered",
            cancellationToken: cancellationToken
            );

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, args) =>
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            var routingKey = args.RoutingKey.ToString();

            switch (routingKey)
            {
                case "product.created":
                    {
                        var integrationEvent = JsonSerializer.Deserialize<ProductCreatedIntegrationEvent>(message);

                        if(integrationEvent is null)
                            return;

                        Console.WriteLine("========== Product Created ==========");
                        Console.WriteLine($"Id         : {integrationEvent.ProductId}");
                        Console.WriteLine($"Name       : {integrationEvent.Name}");
                        Console.WriteLine($"Price      : {integrationEvent.Price}");
                        Console.WriteLine($"Currency   : {integrationEvent.Currency}");
                        Console.WriteLine("=====================================");

                        break;
                    }

                case "user.registered":
                    {
                        var integrationEvent = JsonSerializer.Deserialize<UserRegisteredIntegrationEvent>(message);

                        if(integrationEvent is null)
                            return;

                        Console.WriteLine("========== User Registered ==========");
                        Console.WriteLine($"Id         : {integrationEvent.UserId}");
                        Console.WriteLine($"Name       : {integrationEvent.Email}");
                        Console.WriteLine("=====================================");

                        break;
                    }
                    default:
                     Console.WriteLine($"Unkown routing key: {routingKey}");

                     break;
            }

            await channel.BasicAckAsync(
                deliveryTag: args.DeliveryTag,
                multiple: false,
                cancellationToken: cancellationToken);
        };

        await channel.BasicConsumeAsync(
            queue: "notification-service",
            autoAck: false,
            consumer: consumer,
            cancellationToken: cancellationToken);

        await Task.Delay(Timeout.Infinite, cancellationToken);

    }
}
