using BookStore.ProductService.Application.Abstraction.Messaging;
using BookStore.ProductService.Application.Events.Integration;
using BookStore.ProductService.Domain.Events;
using BookStore.ProductService.Domain.Products;
using BookStore.BuildingBlocks.Messaging;

public sealed class IntegrationEventMapper: IIntegrationEventMapper
{
    public IntegrationEvent? Map(IDomainEvent domainEvent)
    {
        return domainEvent switch
        {
            ProductCreatedEvent e=> new ProductCreatedIntegrationEvent(
                e.ProductId,
                e.Name,
                e.Price,
                e.Currency
            ),

        };
    }
}
