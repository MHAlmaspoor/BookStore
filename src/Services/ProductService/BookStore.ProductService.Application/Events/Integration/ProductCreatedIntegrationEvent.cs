using BookStore.ProductService.Application.Abstraction.Messaging;

namespace BookStore.ProductService.Application.Events.Integration;

public sealed record ProductCreatedIntegrationEvent(
    Guid ProductId,
    string Name,
    decimal Price,
    string Currency) :IntegrationEvent;


