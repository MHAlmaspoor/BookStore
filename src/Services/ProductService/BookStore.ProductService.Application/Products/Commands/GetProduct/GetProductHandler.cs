using BookStore.ProductService.Application.Abstraction.Caching;
using BookStore.ProductService.Application.Abstraction.Persistence;
using BookStore.ProductService.Domain.Products;
using MediatR;

namespace BookStore.ProductService.Application.Products.Command.GetProduct;

public sealed class GetProductHandler : IRequestHandler<GetProductQuery, ProductResponse>
{
    private readonly IProductRepository _repository;
    private readonly IProductCache _cache;

    public GetProductHandler(IProductRepository repository, IProductCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<ProductResponse?> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {

        var cachedProduct = await _cache.GetAsync(request.productId, cancellationToken);

        if(cachedProduct is not null)
            return cachedProduct;

        var product = await _repository.GetByIdAsync(request.productId, cancellationToken);

        if(product is null)
            return null;

        var response = new ProductResponse(product.Id, product.Name, product.Price.Amount, product.Price.Currency);

        await _cache.SetAsync(response, cancellationToken);

        return response;
    }
}
