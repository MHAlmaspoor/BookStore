using BookStore.ProductServicec.Domain.Events;

namespace BookStore.ProductServicec.Domain.Products;

public sealed class ProductCreatedEvent:IDomainEvent
{
    public Guid ProductId { get; }
    public DateTime OccurredOn { get; }

    public ProductCreatedEvent(Guid productId)
    {
        ProductId=productId;
        OccurredOn=DateTime.Now;
    }
}
