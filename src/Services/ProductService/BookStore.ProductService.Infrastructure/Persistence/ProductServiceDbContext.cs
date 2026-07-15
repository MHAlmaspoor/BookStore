using BookStore.ProductService.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace BookStore.ProductService.Infrastructure.Persistence;

public sealed class ProductServiceDbContext : DbContext
{
    public ProductServiceDbContext(DbContextOptions<ProductServiceDbContext> options)
        :base(options)
    {

    }

    public DbSet<Product> Products=>Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductServiceDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
