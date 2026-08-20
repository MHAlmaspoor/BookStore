using BookStore.ProductService.Infrastructure.Persistence;
using BookStore.BuildingBlocks.Persistence.Outbox;
using BookStore.ProductService.Infrastructure.Persistence.Repositories;
using BookStore.ProductService.Application.Abstraction.Persistance;
using BookStore.ProductService.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BookStore.BuildingBlocks.Infrastructure.Messaging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using BookStore.BuildingBlocks.Messaging;
using BookStore.ProductService.Application.Abstraction.Messaging;
using BookStore.ProductService.Application.Abstraction.Caching;
using BookStore.ProductService.Infrastructure.Caching;
using Polly;
using Polly.Retry;
using StackExchange.Redis;
using Polly.CircuitBreaker;

namespace BookStore.ProductService.Infrastructure;

public static class DependenctInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IEventBus,RabbitMqEventBus>();
        services.Configure<RabbitMqOptions>(
        configuration.GetSection(RabbitMqOptions.SectionName));
    services.AddSingleton<IRabbitMqConnection>(sp =>
{
    var settings = sp
        .GetRequiredService<IOptions<RabbitMqOptions>>()
        .Value;

    var factory = new ConnectionFactory
    {
        HostName = settings.Host,
        Port = settings.Port,
        UserName = settings.Username,
        Password = settings.Password,

        AutomaticRecoveryEnabled = true,
        RequestedHeartbeat = TimeSpan.FromSeconds(30)
    };

    var connection = factory
        .CreateConnectionAsync("ProductService")
        .GetAwaiter()
        .GetResult();

    return new RabbitMqConnection(connection);
});
        services.AddSingleton<InsertOutboxMessagesInterceptor>();
        services.AddDbContext<ProductServiceDbContext>((sp, option)=>
        {
            option.UseNpgsql(configuration.GetConnectionString("ProductDatabase"));

            option.AddInterceptors(sp.GetRequiredService<InsertOutboxMessagesInterceptor>());

    });

        services.AddHostedService<OutboxProcessor>();
Console.WriteLine(">>> OUTBOX PROCESSOR REGISTERED");

        services.AddScoped<IOutboxDbContext>(sp =>sp.GetRequiredService<ProductServiceDbContext>());
        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<InsertOutboxMessagesInterceptor>();

        services.AddSingleton<IIntegrationEventMapper,IntegrationEventMapper>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
        });
        services.AddScoped<IProductCache, RedisProductCache>();

        services.AddResiliencePipeline("redis", builder =>
        {
            builder.AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 2,
                Delay = TimeSpan.FromMicroseconds(200),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,

                ShouldHandle = new PredicateBuilder()
                    .Handle<RedisConnectionException>()
                    .Handle<RedisTimeoutException>()
            });

            builder.AddTimeout(TimeSpan.FromSeconds(2));

            builder.AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                FailureRatio = 0.5,
                SamplingDuration = TimeSpan.FromSeconds(30),
                MinimumThroughput = 5,
                BreakDuration = TimeSpan.FromSeconds(15)
            });
        });

        return services;

    }

}
