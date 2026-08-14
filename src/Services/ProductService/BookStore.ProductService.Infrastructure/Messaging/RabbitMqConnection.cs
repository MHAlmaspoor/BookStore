// using System.Data;
// using Microsoft.Extensions.Options;
// using RabbitMQ.Client;

// namespace BookStore.ProductService.Infrastructure.Messaging;

// public sealed class RabbitMqConnection : IRabbitMqConnection
// {
//     public IConnection Connection { get; }

//     public RabbitMqConnection(IConnection connection)
//     {
//         Connection = connection;
//     }

//     public async ValueTask DisposeAsync()
//     {
//         await Connection.DisposeAsync();
//     }


// }
