using BookStore.IdentityService.Application.Abstractions.Authorization;
using BookStore.IdentityService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace BookStore.IdentityService.Infrastructure.Persistence.Repositories;

internal sealed class UserPermissionRepository : IUserPermissionRepository
{
    private readonly IdentityDbContext _context;

    public UserPermissionRepository(IdentityDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<string>> GetPermissionsAsync( UserId userId, CancellationToken cancellationToken = default)
    {


        return await _context.UserRoles
        .Where(ur => ur.UserId == userId)
        .SelectMany(ur=>ur.Role.Permissions)
        .Select(rp=>rp.Permission.Name)
        .Distinct()
        .ToListAsync(cancellationToken);
    }
}
