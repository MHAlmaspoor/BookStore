using BookStore.IdentityService.Domain.RefreshTokens;
using BookStore.IdentityService.Domain.ValueObjects;

namespace BookStore.IdentityService.Application.Abstraction.Authentication;

public interface IRefreshTokenGenerator
{
    RefreshToken Generate(UserId userId);
}
