using BookStore.IdentityService.Domain.Common;

namespace BookStore.IdentityService.Domain.Permissions;

public sealed class Permission : Entity<PermissionId>
{
    public string Name { get; private set; } = null!;

    private Permission() : base(PermissionId.New())
    {
    }

    private Permission(PermissionId id, string name) : base(id)
    {
        Name = name.Trim();
    }

    public static Permission Create(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new Permission(PermissionId.New(), name);
    }
}
