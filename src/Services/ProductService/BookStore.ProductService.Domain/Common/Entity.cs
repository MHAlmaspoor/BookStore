namespace BookStore.ProductServicec.Domain.Common;
public abstract class Entity
{
    public Guid Id{get; protected set;}
    protected Entity()
    {
        Id=Guid.CreateVersion7();
    }
    public bool Equals(Entity? other)
    {
        if(other is null)
            return false;
        if(ReferenceEquals(this,other))
            return true;
        if(GetType()!=other.GetType())
            return false;
        return Id==other.Id;
    }
    public override bool Equals(object? obj)
    {
        //1
        //return Equals(obj as Entity)    1===2

        //2
        return obj is Entity other && Equals(other);
    }
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    public static bool operator ==(Entity? left,Entity? right)
    {
        return Equals(left,right);
    }
    public static bool operator !=(Entity? left, Entity? right)
    {
        return !Equals(left,right);
    }
}
