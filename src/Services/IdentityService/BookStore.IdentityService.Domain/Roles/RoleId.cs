namespace BookStore.IdentityService.Domain.Roles;

public sealed record RoleId(Guid Value)
{
    public static RoleId New() => new(Guid.CreateVersion7());
    public override string ToString() => Value.ToString();
    
    public static implicit operator Guid(RoleId id) => id.Value;
}
