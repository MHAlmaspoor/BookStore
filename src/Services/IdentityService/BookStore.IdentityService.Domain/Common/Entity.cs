using System.Diagnostics;

namespace BookStore.IdentityService.Domain.Common;
public abstract class Entity<TId>:IEquatable<Entity<TId>> where TId : notnull
{
    public TId Id { get; private set; } = default!;

    protected Entity()
    {

    }
    public Entity(TId id)
    {
        Id =id;
    }
    public bool Equals(Entity<TId>? other)
    {
        if(other is null)
            return false;
        if(ReferenceEquals(this,other))
            return true;
        if(GetType()!=other.GetType())
            return false;
        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }
    public override bool Equals(object? obj) =>obj is Entity<TId> other && Equals(other);
    // {
    //     //1
    //     //return Equals(obj as Entity)    1===2

    //     //2
    //     return obj is Entity other && Equals(other);
    // }
    public override int GetHashCode() => EqualityComparer<TId>.Default.GetHashCode(Id);

    public static bool operator ==(Entity<TId> left, Entity<TId> right) => Equals(left,right);

    public static bool operator !=(Entity<TId> left, Entity<TId> right) => !Equals(left, right);
}
