using BookStore.BuildingBlocks.Messaging;
using BookStore.NotificationService.Application.Abstractions.Messaging;

namespace BookStore.NotificationService.Contracts;

public sealed class IntegrationEventRegistry : IIntegrationEventRegistry
{
    private readonly Dictionary<string, Type> _eventTypes = new (StringComparer.OrdinalIgnoreCase);

    public void Register<TEvent>(string routingKey) where TEvent : IntegrationEvent
    {
        if(string.IsNullOrWhiteSpace(routingKey))
            throw new ArgumentException("Routing key cannot be empty.");

        _eventTypes[routingKey] = typeof(TEvent);
    }
    public Type? GetEventType(string routingKey)
    {
        return _eventTypes.TryGetValue(routingKey, out var eventType) ? eventType : null;
    }
}
