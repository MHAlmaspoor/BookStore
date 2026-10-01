using MediatR;

namespace BookStore.ProductService.Application.Products.Command.ChangeProductPrice;

public sealed record ChangeProductPriceCommand(Guid ProdcutId, decimal Price, string Currency) : IRequest;

