using BookStore.BuildingBlocks.Messaging;

namespace BookStore.IdentityService.Application.Messaging.Events;

public sealed record UserRegisteredIntegrationEvent(Guid UserId, string Email) : IntegrationEvent
{
    public override string RoutingKey => IdentityRoutingKeys.UserRegistered;
}
