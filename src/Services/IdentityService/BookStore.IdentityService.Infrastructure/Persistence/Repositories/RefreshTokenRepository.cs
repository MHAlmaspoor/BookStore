using BookStore.IdentityService.Application.Abstraction.Authentication;
using BookStore.IdentityService.Domain.RefreshTokens;
using BookStore.IdentityService.Domain.Users;
using BookStore.IdentityService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace BookStore.IdentityService.Infrastructure.Persistence.Repositories;

internal sealed class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IdentityDbContext _context;

    public RefreshTokenRepository(IdentityDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
    }

    public async Task<RefreshToken> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _context.RefreshTokens.FirstOrDefaultAsync(x=>x.Token == token, cancellationToken);
    }

    public Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        _context.RefreshTokens.Update(refreshToken);
        return Task.CompletedTask;
    }

    public async Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(x=>x.Id == userId,cancellationToken);
    }
}
