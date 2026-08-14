using System.Text.Json;
using BookStore.BuildingBlocks.Messaging;
using BookStore.BuildingBlocks.Persistence.Outbox;
using BookStore.IdentityService.Application.Messaging.Events;

using BookStore.IdentityService.Domain.Users.Events;

namespace BookStore.IdentityService.Application.Messaging;

public sealed class IntegrationEventMapper
    : IIntegrationEventMapper
{
    public IntegrationEvent? Map(
        OutboxMessage message)
    {
        var eventType = Type.GetType(message.Type);

        if (eventType is null)
            return null;

        var domainEvent = JsonSerializer.Deserialize(
            message.Content,
            eventType,
            new JsonSerializerOptions(
                JsonSerializerDefaults.Web));

        return domainEvent switch
        {
            UserRegisteredEvent e =>
                new UserRegisteredIntegrationEvent(
                    e.UserId,
                    e.Email),

            _ => null
        };
    }
}
