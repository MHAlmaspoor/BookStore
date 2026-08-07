using BookStore.IdentityService.Domain.Common;
using BookStore.IdentityService.Domain.Permissions;
using BookStore.IdentityService.Domain.RolePermissions;
using BookStore.IdentityService.Domain.UserRoles;

namespace BookStore.IdentityService.Domain.Roles;


public sealed class Role : Entity<RoleId>
{
    public string Name { get; private set; } = null!;
    private readonly List<UserRole> _users = [];
    private readonly List<RolePermission> _permissions = [];

    public IReadOnlyCollection<UserRole> Users => _users.AsReadOnly();
    public IReadOnlyCollection<RolePermission> Permissions => _permissions.AsReadOnly();


    private Role()
    {

    }

    private Role(RoleId id, string name): base(id)
    {
        Name=name.Trim();
    }

    public static Role Create(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new Role(RoleId.New(), name);
    }

    public void Rename(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        name=name.Trim();

        if(Name==name)
            return;

        Name =name;
    }

    public void AddPermission(PermissionId permissionId)
    {
        if(_permissions.Any(x=>x.PermissionId == permissionId))
            return;

        _permissions.Add(RolePermission.Create(Id, permissionId));
    }

    public void RemovePermission(PermissionId permissionId)
    {
        var permission = _permissions.FirstOrDefault(x=>x.PermissionId == permissionId);

        if(permission is null)
            return;

        _permissions.Remove(permission);
    }
}
