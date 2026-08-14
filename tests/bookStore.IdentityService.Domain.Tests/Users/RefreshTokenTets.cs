using BookStore.IdentityService.Domain.RefreshTokens;
using BookStore.IdentityService.Domain.ValueObjects;
using FluentAssertions;

namespace BookStore.IdentityService.Domain.Tests.Users;

public class RefreshTokenTests
{
    // [Fact]
    // public void Create_Should_Create_Active_Token()
    // {
    //     var token = new RefreshToken(
    //         new UserId(),
    //         "token",
    //         DateTime.UtcNow.AddDays(7),
    //         DateTime.UtcNow,
    //         "Chrome",
    //         "127.0.0.1");

    //     token.IsActive.Should().BeTrue();
    //     token.IsRevoked.Should().BeFalse();
    //     token.IsExpired.Should().BeFalse();
    // }

    // [Fact]
    // public void Revoke_Should_Revoke_Token()
    // {
    //     var token = new RefreshToken(
    //         new UserId(),
    //         "token",
    //         DateTime.UtcNow.AddDays(7),
    //         DateTime.UtcNow,
    //         null,
    //         null);

    //     token.Revoke();

    //     token.IsRevoked.Should().BeTrue();
    //     token.IsActive.Should().BeFalse();
    // }
}
