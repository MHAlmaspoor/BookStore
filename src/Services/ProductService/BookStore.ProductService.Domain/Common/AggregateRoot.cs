using BookStore.BuildingBlocks.Domain;

namespace BookStore.ProductService.Domain.Common;

public abstract class AggregatedRoot:Entity, IAggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents=new ();
    public IReadOnlyCollection<IDomainEvent> DomainEvents =>_domainEvents.AsReadOnly();
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

}
