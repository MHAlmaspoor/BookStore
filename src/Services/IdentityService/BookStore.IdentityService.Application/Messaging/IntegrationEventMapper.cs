using BookStore.BuildingBlocks.Messaging;
using BookStore.IdentityService.Application.Messaging.Events;
using BookStore.IdentityService.Domain.Events;
using BookStore.IdentityService.Domain.Users.Events;

namespace BookStore.IdentityService.Application.Messaging;

public sealed class IntegrationEventMapper : IIntegrationEventMapper
{
    public IntegrationEvent? Map(IDomainEvent domainEvent)
    {
        return domainEvent switch
        {
            UserRegisteredEvent e=> new UserRegisteredIntegrationEvent(e.UserId, e.Email), _=>null
        };
    }
}
