using BookStore.ProductService.Application.Abstraction.Persistance;
using BookStore.ProductService.Application.Products.Command.CreateProduct;
using BookStore.ProductService.Domain.Products;
using BookStore.ProductService.Domain.ValueObjects;
using MediatR;

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(IProductRepository repository, IUnitOfWork unitOfWork)
    {
        _repository=repository;
        _unitOfWork=unitOfWork;
    }
    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var price=new Money(request.Price,request.Currency);
        var product=new Product(request.Name,price);
        await _repository.AddAsync(product,cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return product.Id;
    }

}
