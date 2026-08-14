using MediatR;


using BookStore.BuildingBlocks.Domain;

namespace BookStore.ProductService.Application.Abstraction.Messaging;

public sealed class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IMediator _mediator;

    public DomainEventDispatcher(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task DispatchAsync(
        IReadOnlyCollection<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(
                domainEvent,
                cancellationToken);
        }
    }
}
