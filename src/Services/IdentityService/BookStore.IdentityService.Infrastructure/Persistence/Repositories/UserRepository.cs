using BookStore.IdentityService.Application.Abstractions.Persistence;
using BookStore.IdentityService.Domain.Users;
using BookStore.IdentityService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;


namespace BookStore.IdentityService.Infrastructure.Persistence.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly IdentityDbContext _context;

    public UserRepository(IdentityDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
    }

    public async Task<bool> ExistsAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await _context.Users.AnyAsync(x=>x.Email.Value==email.Value, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await _context.Users.Include(x=>x.RefreshTokens)
            .SingleOrDefaultAsync(x=>x.Email.Value==email.Value,cancellationToken);
    }

    public async Task<User?> GetByIdAsync(UserId id,CancellationToken cancellationToken = default)
    {
        return await _context.Users.Include(x=>x.RefreshTokens)
            .SingleOrDefaultAsync(x=>x.Id == id,cancellationToken);
    }
}
