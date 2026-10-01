using BookStore.BuildingBlocks.Domain;

namespace BookStore.ProductService.Application.Abstraction.Messaging;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(
        IReadOnlyCollection<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}
