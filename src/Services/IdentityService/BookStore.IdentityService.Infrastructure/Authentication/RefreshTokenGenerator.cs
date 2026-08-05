using System.Security.Cryptography;
using BookStore.IdentityService.Application.Abstraction.Authentication;
using BookStore.IdentityService.Domain.RefreshTokens;
using BookStore.IdentityService.Domain.ValueObjects;

namespace BookStore.IdentityService.Infrastructure.Authentication;

internal sealed class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public RefreshToken Generate(UserId userId)
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        return RefreshToken.Create(userId, token, DateTime.UtcNow.AddDays(30));
    }
}
