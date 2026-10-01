
using System.Text.Json.Serialization;
using BookStore.BuildingBlocks.Domain;

namespace BookStore.ProductService.Domain.Products;

public sealed class ProductCreatedEvent : IDomainEvent
{
    public Guid ProductId { get; }
    public DateTime OccurredOn { get; }
    public string Name { get; }
    public decimal Price { get; }
    public string Currency { get; }

    [JsonConstructor]
    public ProductCreatedEvent(
        Guid productId,

        string name,
        decimal price,
        string currency)
    {
        ProductId = productId;
        Name = name;
        Price = price;
        Currency = currency;
    }

    public ProductCreatedEvent(
        Guid productId,
        DateTime occurredOn,
        string name,
        decimal price,
        string currency)
    {
        ProductId = productId;
        OccurredOn = occurredOn;
        Name = name;
        Price = price;
        Currency = currency;
    }
}
