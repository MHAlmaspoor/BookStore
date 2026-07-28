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

        Console.WriteLine(integrationEvent?.GetType().FullName);

        Console.WriteLine(
            System.Text.Json.JsonSerializer.Serialize(
                integrationEvent,
                integrationEvent!.GetType()));

        if(integrationEvent is null)
            return;
        Console.WriteLine("===== Before Publish =====");
        Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(integrationEvent));
        Console.WriteLine("==========================");
        await _eventBus.PublishAsync(integrationEvent,cancellationToken);
    }
}
