using BookStore.IdentityService.Domain.Events;
namespace BookStore.IdentityService.Domain.Common;

public interface IHasDomainEvent
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}
