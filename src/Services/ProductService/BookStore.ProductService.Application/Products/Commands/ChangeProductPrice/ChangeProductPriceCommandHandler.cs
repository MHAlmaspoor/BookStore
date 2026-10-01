using BookStore.ProductService.Application.Abstraction.Caching;
using BookStore.ProductService.Application.Abstraction.Persistance;
using BookStore.ProductService.Domain.Common;
using BookStore.ProductService.Domain.Products;
using BookStore.ProductService.Domain.ValueObjects;
using MediatR;

namespace BookStore.ProductService.Application.Products.Command.ChangeProductPrice;

public sealed class ChangeProductPriceCommandHandler : IRequestHandler<ChangeProductPriceCommand>
{
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductCache _cache;

    public ChangeProductPriceCommandHandler(IProductRepository repository,
        IUnitOfWork unitOfWork, IProductCache cache)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task Handle(ChangeProductPriceCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.ProdcutId, cancellationToken);

        if(product is null)
            throw new KeyNotFoundException($"Product '{request.ProdcutId}' was not found.");

        product.ChangePrice( new Money(request.Price, request.Currency));

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(request.ProdcutId, cancellationToken);
    }
}
