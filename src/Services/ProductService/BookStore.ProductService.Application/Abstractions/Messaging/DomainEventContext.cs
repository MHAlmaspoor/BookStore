using BookStore.BuildingBlocks.Domain;
using BookStore.ProductService.Domain.Common;

namespace BookStore.ProductService.Application.Abstraction.Messaging;

public sealed record DomainEventContext(IAggregateRoot Aggregate, IDomainEvent DomainEvent);
