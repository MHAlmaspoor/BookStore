using System.Security;
using BookStore.IdentityService.Application.Abstraction.Repositories;
using BookStore.IdentityService.Domain.Permissions;
using Microsoft.EntityFrameworkCore;

namespace BookStore.IdentityService.Infrastructure.Persistence.Repositories;

internal sealed class PermissionRepository : IPersmissionRepository
{
    private readonly IdentityDbContext _context;

    public PermissionRepository(IdentityDbContext context)
    {
        _context = context;
    }

    public  Task<Permission?> GetByIdAsync( PermissionId permissionId, CancellationToken cancellationToken = default)
        =>  _context.Permissions.FirstOrDefaultAsync(x=>x.Id == permissionId, cancellationToken);


    public Task<Permission?> GetByNameAsync(string  name, CancellationToken cancellationToken = default )
        => _context.Permissions.FirstOrDefaultAsync(x=>x.Name == name, cancellationToken);

    public Task AddAsync(Permission permission, CancellationToken cancellationToken)
        => _context.Permissions.AddAsync(permission, cancellationToken).AsTask();
}
