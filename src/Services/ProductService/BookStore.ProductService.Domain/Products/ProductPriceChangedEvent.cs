using BookStore.ProductService.Domain.Events;

namespace BookStore.ProductService.Domain.Products;

public sealed class ProductPriceChangedEvent:IDomainEvent
{
    public Guid ProductId { get; }
    public DateTime OccurredOn { get; }

    public ProductPriceChangedEvent(Guid productId)
    {
        ProductId=productId;
        OccurredOn=DateTime.Now;
    }
}
