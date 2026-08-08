using BookStore.IdentityService.Domain.Permissions;
using BookStore.IdentityService.Domain.Roles;
using Microsoft.EntityFrameworkCore;

namespace BookStore.IdentityService.Infrastructure.Persistence.Seed;

internal static class IdentitySeeder
{
    public static async Task SeedAsync(IdentityDbContext context, CancellationToken cancellationToken = default)
    {
        await SeedPermissionsAsync(context, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        await SeedRolesAsync(context, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static async Task SeedPermissionsAsync(IdentityDbContext context, CancellationToken cancellationToken )
    {
        var permissionNames = new[]
        {
            PermissionNames.ProductCreate,
            PermissionNames.ProductUpdate,
            PermissionNames.ProductDelete,

            PermissionNames.UsersCreate,
            PermissionNames.UsersUpdate,
            PermissionNames.UsersDelete,

            PermissionNames.OrdersCreate,
            PermissionNames.OrdersUpdate,
            PermissionNames.OrdersDelete
        };

        foreach (var name in permissionNames)
        {
            var exists = await context.Permissions.AnyAsync( x => x.Name == name, cancellationToken);

            if(!exists)
                context.Permissions.Add(Permission.Create(name));
        }
    }

    private static async Task SeedRolesAsync(IdentityDbContext context, CancellationToken cancellationToken)
    {
        var permissions= await context.Permissions.ToDictionaryAsync(x =>x.Name, cancellationToken);

        await SeedRoleAsync(context, RoleNames.Admin, permissions.Values, cancellationToken);

        await SeedRoleAsync(context, RoleNames.Manager, GetPermissions(permissions,
            PermissionNames.ProductCreate,
            PermissionNames.ProductUpdate,
            PermissionNames.ProductDelete,
            PermissionNames.OrdersCreate,
            PermissionNames.OrdersUpdate,
            PermissionNames.OrdersDelete),cancellationToken);

        await SeedRoleAsync(context, RoleNames.Customer,GetPermissions(permissions, PermissionNames.OrdersCreate), cancellationToken);

    }
    private static async Task SeedRoleAsync(IdentityDbContext context, string roleName,
        IEnumerable<Permission> permissions, CancellationToken cancellationToken)
    {
        var role = await context.Roles.Include(x=>x.Permissions).FirstOrDefaultAsync(x=>x.Name == roleName,cancellationToken);

        if(role is null)
        {
            role = Role.Create(roleName);
            context.Roles.Add(role);
        }

        foreach (var permission in permissions)
            role.AddPermission(permission.Id);
    }

    private static IEnumerable<Permission> GetPermissions(
        Dictionary<string, Permission> permissions, params string[] names)
    {
        foreach (var name in names)
        {
            if (!permissions.TryGetValue(name, out var permission))
                throw new InvalidOperationException($"Permission '{name}' was not found.");

            yield return permission;
        }
    }

}
