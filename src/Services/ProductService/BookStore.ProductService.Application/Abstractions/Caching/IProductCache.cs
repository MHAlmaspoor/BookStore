using BookStore.ProductService.Application.Products.Command.GetProduct;

namespace BookStore.ProductService.Application.Abstraction.Caching;

public interface IProductCache
{
    Task<ProductResponse?> GetAsync(Guid productId, CancellationToken cancellationToken = default);

    Task SetAsync(ProductResponse product, CancellationToken cancellationToken = default);

    Task RemoveAsync(Guid productId, CancellationToken cancellationToken = default);
}
