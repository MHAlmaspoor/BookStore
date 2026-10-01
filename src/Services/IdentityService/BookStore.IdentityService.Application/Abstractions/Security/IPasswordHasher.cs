using BookStore.IdentityService.Domain.ValueObjects;

namespace BookStore.IdentityService.Application.Abstractions.Security;

public interface IPasswordHasher
{
    PasswordHash Hash(string password);

    bool VerifyPassword(string password, PasswordHash hash);

}
