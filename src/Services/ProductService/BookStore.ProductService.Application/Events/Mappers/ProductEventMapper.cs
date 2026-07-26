using BookStore.ProductService.Application.Events.Integration;
using BookStore.ProductService.Domain.Products;

namespace BookStore.ProductService.Application.Events.Mappers;

public static class ProductEventMapper
{
    public static ProductCreatedIntegrationEvent Map(
        ProductCreatedEvent dominEvent, Product product
    )
    {
        return new ProductCreatedIntegrationEvent(
            product.Id,
            product.Name,
            product.Price.Amount,
            product.Price.Currency
        );
    }
}
