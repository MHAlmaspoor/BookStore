using BookStore.IdentityService.Application.Abstraction.Repositories;
using BookStore.IdentityService.Domain.Roles;
using Microsoft.EntityFrameworkCore;

namespace BookStore.IdentityService.Infrastructure.Persistence.Repositories;

internal sealed class RoleRepository : IRoleRepository
{
    private readonly IdentityDbContext _context;

    public RoleRepository(IdentityDbContext context)
    {
        _context = context;
    }

    public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Roles.FirstOrDefaultAsync(x=>x.Name == name,cancellationToken);
    }

    public async Task<Role?> GetByIdAsync(RoleId id, CancellationToken cancellationToken)
    {
        return await _context.Roles.FirstOrDefaultAsync(x=>x.Id == id, cancellationToken);
    }

    public async Task AddAsync(Role role, CancellationToken cancellationToken)
    {
        await _context.Roles.AddAsync(role, cancellationToken);
    }


}
