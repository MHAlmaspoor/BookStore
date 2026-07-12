using BookStore.ProductServicec.Domain.Events;

namespace BookStore.ProductServicec.Domain.Products;

public sealed class ProductRenamedEvent:IDomainEvent
{
    public Guid ProductId { get; }
    public DateTime OccurredOn { get; }

    public ProductRenamedEvent(Guid productId)
    {
        ProductId=productId;
        OccurredOn=DateTime.Now;
    }
}
