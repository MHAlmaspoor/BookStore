using BookStore.ProductService.Domain.Events;

namespace BookStore.ProductService.Application.Abstraction.Messaging;

public interface IIntegrationEventMapper
{
    IntegrationEvent? Map(IDomainEvent domainEvent);
}


