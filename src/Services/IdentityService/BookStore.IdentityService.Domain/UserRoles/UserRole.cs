using BookStore.IdentityService.Domain.Roles;
using BookStore.IdentityService.Domain.ValueObjects;

namespace BookStore.IdentityService.Domain.UserRoles;

public sealed class UserRole
{
    public UserId UserId { get; private set; }
    public RoleId RoleId { get; set; } = null!;

    private UserRole()
    {

    }

    public UserRole(UserId userId, RoleId roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }

    public static UserRole Create(UserId userId, RoleId roleId) =>new(userId,roleId);

}
