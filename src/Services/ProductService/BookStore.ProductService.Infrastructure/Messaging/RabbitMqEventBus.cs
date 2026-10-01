// using System.Text;
// using System.Text.Json;
// using RabbitMQ.Client;
// using BookStore.BuildingBlocks.Messaging;
// namespace BookStore.ProductService.Infrastructure.Messaging;

// public sealed class RabbitMqEventBus:IEventBus
// {
//     private readonly IRabbitMqConnection _connection;
//     public RabbitMqEventBus(IRabbitMqConnection connection)
//     {
//         _connection=connection;
//     }

//     public async Task PublishAsync(IntegrationEvent integrationEvent, CancellationToken cancellationToken=default)
//     {
//         var channel =await _connection.Connection.CreateChannelAsync(cancellationToken: cancellationToken);
//         await channel.ExchangeDeclareAsync(
//             exchange: "bookstore.events",
//             type: ExchangeType.Topic,
//             durable: false,
//             autoDelete: false,
//             cancellationToken: cancellationToken );

//         var body = JsonSerializer.SerializeToUtf8Bytes(
//             integrationEvent,
//             integrationEvent.GetType());
//         var properties = new BasicProperties
//         {
//             Persistent = true,
//             ContentType = "application/json"
//         };

//         const string routingKey = "product.created";
//         await channel.BasicPublishAsync(
//             exchange: "bookstore.events",
//             routingKey: routingKey,
//             mandatory: false,
//             basicProperties: properties,
//             body: body,
//             cancellationToken: cancellationToken);
//     }
// }
