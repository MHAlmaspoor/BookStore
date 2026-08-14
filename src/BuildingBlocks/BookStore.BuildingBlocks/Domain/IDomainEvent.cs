using MediatR;

namespace BookStore.BuildingBlocks.Domain;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
