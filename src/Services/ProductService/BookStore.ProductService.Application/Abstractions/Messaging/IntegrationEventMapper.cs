using BookStore.ProductService.Application.Abstraction.Messaging;
using BookStore.ProductService.Application.Events.Integration;
using BookStore.BuildingBlocks.Domain;
using BookStore.ProductService.Domain.Products;
using BookStore.BuildingBlocks.Messaging;
using BookStore.BuildingBlocks.Persistence.Outbox;
using System.Text.Json;

public sealed class IntegrationEventMapper :  IIntegrationEventMapper
{
    public IntegrationEvent? Map(OutboxMessage message)
    {
        var eventType = Type.GetType(message.Type);

        if (eventType is null)
            return null;

        var domainEvent = JsonSerializer.Deserialize(message.Content, eventType, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        return domainEvent switch
        {
            ProductCreatedEvent e =>
                new ProductCreatedIntegrationEvent(
                    e.ProductId,
                    e.Name,
                    e.Price,
                    e.Currency),

            _ => null
        };
    }
}
