using BookStore.IdentityService.Domain.Users;
using BookStore.IdentityService.Domain.RefreshTokens;
using Microsoft.EntityFrameworkCore;
using BookStore.IdentityService.Application.Abstraction.Persistance;
using BookStore.IdentityService.Domain.Roles;
using BookStore.IdentityService.Domain.UserRoles;
using BookStore.IdentityService.Domain.RolePermissions;
using BookStore.IdentityService.Domain.Permissions;
using BookStore.BuildingBlocks.Persistence.Outbox;

namespace BookStore.IdentityService.Infrastructure.Persistence;

public sealed class IdentityDbContext : DbContext,IUnitOfWork, IOutboxDbContext
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<OutboxMessage> OutboxMessages =>Set<OutboxMessage>();
    public DbSet<Role> Roles =>Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

}
