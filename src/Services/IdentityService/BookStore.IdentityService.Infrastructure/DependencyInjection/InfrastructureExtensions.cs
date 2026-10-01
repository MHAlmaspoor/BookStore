using BookStore.IdentityService.Infrastructure.Persistence;
using BookStore.IdentityService.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.IdentityService.Infrastructure.DependencyInjection;
public static class InfrastructureExtensions
{
    public static async Task SeedInfrastructureAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var context = scope.ServiceProvider
            .GetRequiredService<IdentityDbContext>();

        await context.Database.MigrateAsync();
        
        await IdentitySeeder.SeedAsync(context);
    }

}
