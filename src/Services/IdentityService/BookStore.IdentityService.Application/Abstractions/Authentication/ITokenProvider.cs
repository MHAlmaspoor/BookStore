using BookStore.IdentityService.Application.Contracts.Authentication;
using BookStore.IdentityService.Domain.Users;

namespace BookStore.IdentityService.Application.Abstraction.Authentication;

public interface ITokenProvider
{
    LoginResponse CreateAccessToken(User user);
}
