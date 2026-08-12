using System.Text.Json;
using BookStore.IdentityService.Application.Abstraction.Persistance;
using BookStore.IdentityService.Application.Abstractions.Persistence;
using BookStore.IdentityService.Application.Abstractions.Security;
using BookStore.IdentityService.Infrastructure.Persistence;
using BookStore.IdentityService.Infrastructure.Persistence.Repositories;
using BookStore.IdentityService.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BookStore.IdentityService.Infrastructure.Authentication;
using BookStore.IdentityService.Application.Abstraction.Authentication;
using BookStore.IdentityService.Application.Abstraction.Repositories;
using BookStore.IdentityService.Domain.Roles;
using BookStore.IdentityService.Application.Abstractions.Authorization;
using BookStore.IdentityService.Infrastructure.Persistence.Interceptors;
using BookStore.BuildingBlocks.Messaging;
using BookStore.IdentityService.Application.Messaging;
using BookStore.IdentityService.Infrastructure.BackgroundServices;


namespace BookStore.IdentityService.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure( this IServiceCollection services,IConfiguration configuration)
    {
        services.AddDbContext<IdentityDbContext>(option =>
        {
            option.UseNpgsql(configuration.GetConnectionString("IdentityDatabase"));

            option.AddInterceptors(new InsertOutboxMessagesInterceptor());
        });

        services.AddScoped<IUserRepository,UserRepository>();

        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<IdentityDbContext>());

        services.AddScoped<IPasswordHasher,BCrptPasswordHasher>();
        services.AddScoped<ITokenProvider, JwtTokenProvider>();
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.AddScoped<IRefreshTokenRepository,RefreshTokenRepository>();
        services.AddScoped<IRefreshTokenGenerator,RefreshTokenGenerator>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPersmissionRepository, PermissionRepository>();
        services.AddScoped<IUserPermissionRepository, UserPermissionRepository>();
        services.AddScoped<IIntegrationEventMapper,IntegrationEventMapper>();
        services.AddHostedService<OutboxProcessor>();


        return services;
    }
}
