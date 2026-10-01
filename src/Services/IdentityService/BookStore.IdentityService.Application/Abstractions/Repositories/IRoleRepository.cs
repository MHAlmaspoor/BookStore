using BookStore.IdentityService.Domain.Roles;

namespace BookStore.IdentityService.Application.Abstraction.Repositories;
public interface IRoleRepository
{
    Task<Role?> GetByNameAsync( string name, CancellationToken cancellationToken = default);

    Task<Role?> GetByIdAsync( RoleId id, CancellationToken cancellationToken = default);

    Task AddAsync(Role role, CancellationToken cancellationToken = default);
}
