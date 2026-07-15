using BookStore.ProductService.Infrastructure.Persistence;
using BookStore.ProductService.Infrastructure.Persistence.Interceptors;
using BookStore.ProductService.Infrastructure.Persistence.Repositories;
using BookStore.ProductService.Application.Abstraction.Persistance;
using BookStore.ProductService.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.ProductService.Infrastructure;

public static class DependenctInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<PublishDomainEventInterceptor>();
        services.AddDbContext<ProductServiceDbContext>((sp, option)=>
        {
            option.UseNpgsql(
            configuration.GetConnectionString("ProductDatabase"));

            option.AddInterceptors(sp.GetRequiredService<PublishDomainEventInterceptor>());

            option.AddInterceptors(sp.GetRequiredService<PublishDomainEventInterceptor>());


    });

        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<PublishDomainEventInterceptor>();
        return services;

    }

}
