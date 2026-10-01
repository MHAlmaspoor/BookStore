using BookStore.ProductService.Application.Abstraction.Caching;
using BookStore.ProductService.Application.Abstraction.Persistance;
using BookStore.ProductService.Domain.Products;
using MediatR;

namespace BookStore.ProductService.Application.Products.Command.DeleteProduct;

public sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductCache _cache;

    public DeleteProductCommandHandler (IProductRepository repository,
        IUnitOfWork unitOfWork, IProductCache cache)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.ProductId, cancellationToken);

        if(product is null)
            throw new KeyNotFoundException($"Product '{request.ProductId}' was not found.");

        _repository.Delete(product);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(request.ProductId, cancellationToken);
    }
}
