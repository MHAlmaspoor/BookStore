using BookStore.IdentityService.Domain.Permissions;

namespace BookStore.IdentityService.Application.Abstraction.Repositories;

public interface IPersmissionRepository
{
    Task<Permission?> GetByIdAsync(PermissionId permissionId, CancellationToken cancellationToken = default);

    Task<Permission?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    Task AddAsync(Permission permission, CancellationToken cancellationToken = default);
}
