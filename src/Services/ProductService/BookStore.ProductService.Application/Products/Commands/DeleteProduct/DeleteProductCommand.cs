using MediatR;

namespace BookStore.ProductService.Application.Products.Command.DeleteProduct;

public sealed record DeleteProductCommand(Guid ProductId) : IRequest;
