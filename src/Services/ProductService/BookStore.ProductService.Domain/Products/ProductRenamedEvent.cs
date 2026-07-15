using BookStore.ProductService.Domain.Events;

namespace BookStore.ProductService.Domain.Products;

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
