using MediatR;

namespace BookStore.ProductServicec.Application.Products.Command.CreateProduct;

public sealed record CreateProductCommand(string Name, decimal Price, string Currency):IRequest<Guid>;
