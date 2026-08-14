using BookStore.BuildingBlocks.Messaging;
using BookStore.ProductService.Application.Messaging;

namespace BookStore.ProductService.Application.Events.Integration;

public sealed record ProductCreatedIntegrationEvent(
    Guid ProductId,
    string Name,
    decimal Price,
    string Currency) :IntegrationEvent
{
    public override string RoutingKey => ProductRoutingKeys.Created;
}

