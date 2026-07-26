using BookStore.ProductService.Domain.Events;
using BookStore.ProductService.Domain.Common;

namespace BookStore.ProductService.Application.Abstraction.Messaging;

public sealed record DomainEventContext(IAggregateRoot Aggregate, IDomainEvent DomainEvent);
