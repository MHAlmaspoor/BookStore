using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
namespace BookStore.BuildingBlocks.Infrastructure.DependencyInjection;

using BookStore.BuildingBlocks.Infrastructure.Messaging;
using BookStore.BuildingBlocks.Messaging;
using RabbitMQ.Client;

public static class RabbitMqExtensions
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration
            .GetSection(RabbitMqOptions.SectionName)
            .Get<RabbitMqOptions>()
            ?? throw new InvalidOperationException("RabbitMQ configuration is missing");

        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));

        services.AddSingleton<IConnection>(_=>
        {
            var factory = new ConnectionFactory
            {
                HostName = options.Host,
                Port = options.Port,
                UserName = options.Username,
                Password = options.Password,
            };

            return factory.CreateConnectionAsync().GetAwaiter().GetResult();
        });

        services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();

        services.AddSingleton<IEventBus, RabbitMqEventBus>();;

        return services;
    }
}
