using BookStore.IdentityService.Domain.Users;
using BookStore.IdentityService.Domain.RefreshTokens;
using BookStore.IdentityService.Domain.Outbox;
using Microsoft.EntityFrameworkCore;
using BookStore.IdentityService.Application.Abstraction.Persistance;

namespace BookStore.IdentityService.Infrastructure.Persistence;

public sealed class IdentityDbContext : DbContext,IUnitOfWork
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<OutboxMessage> OutboxMessages =>Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

}
