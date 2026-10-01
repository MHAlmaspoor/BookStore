using BookStore.BuildingBlocks.Messaging;
using BookStore.BuildingBlocks.Persistence.Outbox;

public interface IIntegrationEventMapper
{
    IntegrationEvent? Map(OutboxMessage message);
}
