
using MediatR;

namespace BookStore.IdentityService.Domain.Events;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
