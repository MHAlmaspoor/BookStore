using System.Security.Cryptography.X509Certificates;
using BookStore.ProductService.Domain.Products;

namespace BookStore.ProductService.Application.Abstraction.Persistence;

public interface IProductRepository__
{
    Task AddAsync(Product product);

    Task<Product?> GetByIdAsync(Guid id);
}
