using BookStore.ProductServicec.Domain.Events;

namespace BookStore.ProductServicec.Domain.Common;

public abstract class AggregatedRoot:Entity
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
