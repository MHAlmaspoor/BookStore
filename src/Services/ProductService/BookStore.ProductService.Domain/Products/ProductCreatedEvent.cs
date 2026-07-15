// using BookStore.ProductService.Domain.Events;
// using MediatR;

// namespace BookStore.ProductService.Domain.Products;

// public sealed class ProductCreatedEvent:INotification
// {
//     public Guid ProductId { get; }
//     public DateTime OccurredOn { get; }

//     public ProductCreatedEvent(Guid productId)
//     {
//         ProductId=productId;
//         OccurredOn=DateTime.Now;
//     }
// }

using BookStore.ProductService.Domain.Events;

namespace BookStore.ProductService.Domain.Products;

public sealed class ProductCreatedEvent : IDomainEvent
{
    public Guid ProductId { get; }

    public DateTime OccurredOn { get; }

    public ProductCreatedEvent(Guid productId)
    {
        ProductId = productId;
        OccurredOn = DateTime.UtcNow;
    }
}
