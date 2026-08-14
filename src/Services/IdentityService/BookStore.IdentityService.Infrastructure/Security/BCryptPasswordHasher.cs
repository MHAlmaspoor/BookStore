using BookStore.IdentityService.Application.Abstractions.Security;
using BookStore.IdentityService.Domain.ValueObjects;
using BCrypt.Net;


namespace BookStore.IdentityService.Infrastructure.Security;

internal sealed class BCrptPasswordHasher : IPasswordHasher
{
    public PasswordHash Hash(string password)
    {
        return PasswordHash.Create(BCrypt.Net.BCrypt.HashPassword(password));
    }

    public bool VerifyPassword( string password,PasswordHash hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash.Value);
    }
}
