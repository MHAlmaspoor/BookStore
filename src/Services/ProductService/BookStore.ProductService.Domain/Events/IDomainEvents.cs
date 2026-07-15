// namespace BookStore.ProductService.Domain.Events;

// public interface IDomainEvent
// {
//     DateTime OccurredOn{ get; }
// }

using MediatR;

namespace BookStore.ProductService.Domain.Events;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
