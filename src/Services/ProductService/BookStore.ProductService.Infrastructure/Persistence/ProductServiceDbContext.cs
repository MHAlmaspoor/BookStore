using BookStore.BuildingBlocks.Persistence.Outbox;
using BookStore.ProductService.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace BookStore.ProductService.Infrastructure.Persistence;

public sealed class ProductServiceDbContext : DbContext, IOutboxDbContext
{
    public ProductServiceDbContext(DbContextOptions<ProductServiceDbContext> options)
        :base(options)
    {

    }

    public DbSet<Product> Products=>Set<Product>();
    public DbSet<OutboxMessage> OutboxMessages =>Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductServiceDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
