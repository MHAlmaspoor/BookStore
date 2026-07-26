using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BookStore.ProductService.Infrastructure.Persistence;

public sealed class ProductServiceDbContextFactory
    :IDesignTimeDbContextFactory<ProductServiceDbContext>
{
    public ProductServiceDbContext CreateDbContext(string[] args)
    {
        var optionBuilder = new DbContextOptionsBuilder<ProductServiceDbContext>();

        optionBuilder.UseNpgsql("host=localhost;port=5432;Database=BookStoreProductDb;username=postgres;password=1");

        return new ProductServiceDbContext(optionBuilder.Options);
    }
}
