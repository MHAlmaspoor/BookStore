using BookStore.ProductService.Domain.Events;
namespace BookStore.ProductService.Domain.Common;

public interface IHasDomainEvent
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}
