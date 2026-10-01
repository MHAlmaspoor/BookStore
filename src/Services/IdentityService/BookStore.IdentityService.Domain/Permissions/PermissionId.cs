namespace BookStore.IdentityService.Domain.Permissions;

public sealed record PermissionId(Guid Value)
{
    public static PermissionId New() => new(Guid.CreateVersion7());
    public override string ToString() => Value.ToString();

    public static implicit operator Guid(PermissionId id) => id.Value;

}
