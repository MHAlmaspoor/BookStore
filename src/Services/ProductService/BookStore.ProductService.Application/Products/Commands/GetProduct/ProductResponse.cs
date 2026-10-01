namespace BookStore.ProductService.Application.Products.Command.GetProduct;

public sealed record ProductResponse(Guid Id, string Name, decimal Price, string Currency);
