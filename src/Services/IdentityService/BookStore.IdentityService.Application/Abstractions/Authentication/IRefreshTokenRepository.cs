using BookStore.IdentityService.Domain.RefreshTokens;
using BookStore.IdentityService.Domain.Users;
using BookStore.IdentityService.Domain.ValueObjects;

namespace BookStore.IdentityService.Application.Abstraction.Authentication;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);

    Task<RefreshToken> GetByTokenAsync(string token, CancellationToken cancellationToken = default);

    Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);

    Task<User?> GetByIdAsync(UserId user, CancellationToken cancellationToken = default);
}
