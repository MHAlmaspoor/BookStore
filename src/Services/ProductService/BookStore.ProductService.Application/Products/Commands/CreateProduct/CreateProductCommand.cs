using MediatR;

namespace BookStore.ProductService.Application.Products.Command.CreateProduct;

public sealed record CreateProductCommand(string Name, decimal Price, string Currency):IRequest<Guid>;
