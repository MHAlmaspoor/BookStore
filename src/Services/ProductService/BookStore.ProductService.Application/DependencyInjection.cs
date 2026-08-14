using BookStore.ProductService.Application.Abstraction.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.ProductService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        services.AddSingleton<IIntegrationEventMapper, IntegrationEventMapper>();

        return services;
    }
}
