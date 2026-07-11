namespace BookStore.ProductServicec.Domain.Common;
public abstract class Entity
{
    public Guid Id{get; protected set;}
    protected Entity()
    {
        Id=Guid.CreateVersion7();
    }
}