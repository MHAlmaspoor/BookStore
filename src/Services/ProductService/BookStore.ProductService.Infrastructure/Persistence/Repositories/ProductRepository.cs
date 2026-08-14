using BookStore.ProductService.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace BookStore.ProductService.Infrastructure.Persistence.Repositories;

public sealed class ProductRepository:IProductRepository
{
    private readonly ProductServiceDbContext _context;
    public ProductRepository(ProductServiceDbContext context)
    {
        _context=context;
    }
    public async Task AddAsync(Product product,CancellationToken cancellationToken=default)
    {
        ArgumentNullException.ThrowIfNull(product);
        await _context.Products.AddAsync(product,cancellationToken);
    }
    public async Task<Product?> GetByIdAsync(Guid id,CancellationToken cancellationToken)
    {
        return await _context.Products.FindAsync(id,cancellationToken);
    }
}
