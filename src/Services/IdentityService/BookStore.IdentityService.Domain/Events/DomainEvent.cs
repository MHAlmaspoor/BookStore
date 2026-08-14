using BookStore.BuildingBlocks.Domain;

namespace BookStore.IdentityService.Domain.Events;

public abstract record DomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
