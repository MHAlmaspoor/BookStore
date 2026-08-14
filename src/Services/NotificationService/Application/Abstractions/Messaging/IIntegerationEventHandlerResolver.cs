namespace BookStore.NotificationService.Application.Abstractions.Messaging;

public interface IIntegrationEventHandlerResolver
{
    Task HandleAsync(string routingKey, string message, CancellationToken cancellationToken = default);
}
