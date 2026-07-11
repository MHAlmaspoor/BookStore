namespace BookStore.ProductServicec.Domain.Events;

public interface IDomainEvent
{
    DateTime OccurredOn{ get; }
}
