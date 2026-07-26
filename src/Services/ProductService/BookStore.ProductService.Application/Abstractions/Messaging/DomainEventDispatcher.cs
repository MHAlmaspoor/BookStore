using BookStore.ProductService.Domain.Events;
using MediatR;

namespace BookStore.ProductService.Application.Abstraction.Messaging;

public sealed class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IPublisher _publisher;
    private readonly IIntegrationEventMapper _eventMapper;
    private readonly IEventBus _eventBus;

    public DomainEventDispatcher(IPublisher publisher, IIntegrationEventMapper eventMapper, IEventBus eventBus)
    {
        _publisher=publisher;
        _eventMapper=eventMapper;
        _eventBus=eventBus;

    }

    public async Task DispatchAsync(DomainEventContext context, CancellationToken cancellationToken=default)
    {
        await _publisher.Publish( context.DomainEvent,cancellationToken);
        var integrationEvent = _eventMapper.Map(context.DomainEvent);

        if(integrationEvent is null)
            return;

        await _eventBus.PublishAsync(integrationEvent,cancellationToken);
    }
}
