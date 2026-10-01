using BookStore.IdentityService.Domain.Roles;
using BookStore.IdentityService.Domain.Users;
using BookStore.IdentityService.Domain.ValueObjects;

namespace BookStore.IdentityService.Domain.UserRoles;

public sealed class UserRole
{
    public UserId UserId { get; private set; }
    public User User { get; private set; } = null!;
    public RoleId RoleId { get; private set; }

    public Role Role { get; private set;} = null!;

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
