using BookStore.IdentityService.Domain.Events;
using BookStore.BuildingBlocks.Messaging;

namespace BookStore.BuildingBlocks.Messaging;

public interface IIntegrationEventMapper
{
    IntegrationEvent? Map(IDomainEvent domainEvent);
}
