using BookStore.BuildingBlocks.Messaging;
namespace BookStore.NotificationService.Application.Abstractions.Messaging;

public interface IIntegrationEventHandler<in TEvent> where TEvent : IntegrationEvent
{
    Task HandleAsync(TEvent integrationEvent , CancellationToken cancellationToken = default);
}
