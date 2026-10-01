using BookStore.IdentityService.Domain.Permissions;
using BookStore.IdentityService.Domain.Roles;

namespace BookStore.IdentityService.Domain.RolePermissions;

public sealed class RolePermission
{
    public RoleId RoleId { get; private set; } = null!;
    public Role Role { get; set; } = null!;
    public PermissionId PermissionId { get; private set; } = null!;
    public Permission Permission { get; set; } = null!;

    private RolePermission()
    {

    }

    public RolePermission(RoleId roleId, PermissionId permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }

    public static RolePermission Create(RoleId roleId, PermissionId permissionId) => new(roleId, permissionId);
}
