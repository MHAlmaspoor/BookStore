namespace BookStore.ProductService.Application.Products.Command.ChangeProductPrice;

public sealed record ChangeProductPriceRequest(decimal Price, string Currency);
