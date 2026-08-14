using BookStore.BuildingBlocks.Domain;
namespace BookStore.BuildingBlocks.Domain;

public interface IHasDomainEvent
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}
