using BookStore.ProductService.Domain.Events;

namespace BookStore.ProductService.Application.Abstraction.Messaging;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(DomainEventContext context, CancellationToken cancellationToken=default);
}
