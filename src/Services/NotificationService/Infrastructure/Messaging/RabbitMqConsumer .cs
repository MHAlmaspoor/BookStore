using System.Diagnostics;
using System.Text;
using BookStore.BuildingBlocks.Messaging;
using BookStore.NotificationService.Application.Abstractions.Messaging;
using OpenTelemetry.Context.Propagation;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BookStore.NotificationService.Infrastructure.Messaging;

public sealed class RabbitMqConsumer : BackgroundService
{
    private const string QueueName = "notification-service";
    private const string ExchangeName = "bookstore.events";

    private static readonly ActivitySource ActivitySource = new("BookStore.NotificationService.RabbitMQ");
    private static readonly TextMapPropagator Propagator = Propagators.DefaultTextMapPropagator;

    private readonly RabbitMqConnection _connection;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<RabbitMqConsumer> _logger;

    public RabbitMqConsumer(RabbitMqConnection connection, IServiceScopeFactory scopeFactory, ILogger<RabbitMqConsumer> logger)
    {
        _connection = connection;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Notification RabbitMQ Consumer started.");

        await using var channel = await _connection.Connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.QueueDeclareAsync(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: stoppingToken);

        await channel.ExchangeDeclareAsync(
            exchange: ExchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false,
            cancellationToken: stoppingToken);

        await channel.QueueBindAsync(
            queue: QueueName,
            exchange: ExchangeName,
            routingKey: RabbitMqRoutingKeys.ProductCreated,
            cancellationToken: stoppingToken);

        await channel.QueueBindAsync(
            queue: QueueName,
            exchange: ExchangeName,
            routingKey: RabbitMqRoutingKeys.UserRegistered,
            cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (_, args) =>
        {
            var parentContext = Propagator.Extract(default, args.BasicProperties, static(properties, key) =>
            {
                if(properties.Headers is null)
                    return [];
                if(!properties.Headers.TryGetValue(key, out var value))
                    return [];
                return value switch
                {
                    byte[] bytes => [Encoding.UTF8.GetString(bytes)], string text => [text], _ => []
                };
            });

            using var activity = ActivitySource.StartActivity($"RabbitMQ {args.RoutingKey}", ActivityKind.Consumer,
                parentContext.ActivityContext);

            activity?.SetTag("messaging.system", "rabbitmq");
            activity?.SetTag("messaging.destination.name", ExchangeName);
            activity?.SetTag("messaging.operation.type", "process");
            activity?.SetTag("messaging.destination.kind", "topic");
            activity?.SetTag("messaging.rabbitmq.destination.routing_key", args.RoutingKey);
            try
            {
                var body = args.Body.ToArray();

                var message = Encoding.UTF8.GetString(body);

                var routingKey = args.RoutingKey;

                _logger.LogInformation("Received RabbitMQ message. RoutingKey: {RoutingKey}", routingKey);

                using var scope = _scopeFactory.CreateScope();

                var resolver = scope.ServiceProvider.GetRequiredService<IIntegrationEventHandlerResolver>();

                await resolver.HandleAsync(routingKey, message, stoppingToken);

                await channel.BasicAckAsync(deliveryTag: args.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing RabbitMQ message. RoutingKey: {RoutingKey}", args.RoutingKey);

                await channel.BasicNackAsync(
                    deliveryTag: args.DeliveryTag,
                    multiple: false,
                    requeue: false,
                    cancellationToken: stoppingToken);
            }
        };

        await channel.BasicConsumeAsync(
            queue: QueueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);

        _logger.LogInformation("RabbitMQ Consumer is listening on queue {QueueName}.", QueueName);

        try
        {
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
            when (stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Notification RabbitMQ Consumer is stopping.");
        }
    }
}
