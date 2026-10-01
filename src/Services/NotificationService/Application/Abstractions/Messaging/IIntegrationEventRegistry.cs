namespace BookStore.NotificationService.Application.Abstractions.Messaging;

public interface IIntegrationEventRegistry
{
    Type? GetEventType(string routingKey);
}
