using BookStore.ProductService.Infrastructure.Persistence;
using BookStore.ProductService.Infrastructure.Persistence.Interceptors;
using BookStore.ProductService.Infrastructure.Persistence.Repositories;
using BookStore.ProductService.Application.Abstraction.Persistance;
using BookStore.ProductService.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BookStore.ProductService.Infrastructure.Messaging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using BookStore.ProductService.Application.Abstraction.Messaging;

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
        services.AddScoped<PublishDomainEventInterceptor>();
        services.AddDbContext<ProductServiceDbContext>((sp, option)=>
        {
            option.UseNpgsql(
            configuration.GetConnectionString("ProductDatabase"));

            option.AddInterceptors(sp.GetRequiredService<PublishDomainEventInterceptor>());


    });

        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<PublishDomainEventInterceptor>();

        services.AddScoped<IIntegrationEventMapper,IntegrationEventMapper>();
        return services;

    }

}
