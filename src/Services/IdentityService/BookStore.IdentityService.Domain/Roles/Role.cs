using BookStore.IdentityService.Domain.Common;

namespace BookStore.IdentityService.Domain.Roles;


public sealed class Role : Entity<RoleId>
{
    public string Name { get; private set; } = null!;

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
}
