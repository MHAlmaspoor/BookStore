using Microsoft.Extensions.DependencyInjection;

namespace BookStore.ProductService.Application;

public static class DependencyInjection
{
    public static IServiceCollection addApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg=>cfg.RegisterServicesFromAssembly(
            typeof(DependencyInjection).Assembly));
        return services;
    }
}
