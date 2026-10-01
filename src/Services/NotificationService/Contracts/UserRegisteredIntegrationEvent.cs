using BookStore.BuildingBlocks.Messaging;

namespace BookStore.NotificationService.Contracts;

public sealed record UserRegisteredIntegrationEvent(Guid UserId, string Email) : IntegrationEvent
{
    public override string RoutingKey => RabbitMqRoutingKeys.UserRegistered;
}
