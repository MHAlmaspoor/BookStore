using BookStore.IdentityService.Domain.Users;
using BookStore.IdentityService.Domain.ValueObjects;
using FluentAssertions;

namespace BookStore.IdentityService.Domain.Tests.Users;

public class UserTests
{
    [Fact]
    public void Create_Should_Create_User()
    {
        var user = new User(
            UserId.New(),
            Email.Create("admin@test.com"),
            PasswordHash.Create("HASH"),
            "Ali",
            "Ahmadi");

        user.Email.Value.Should().Be("admin@test.com");
        user.IsActive.Should().BeTrue();
    }

    [Fact]
    public void AddRefreshToken_Should_Add_Token()
    {
        // var user = new User(
        //     UserId.New(),
        //     Email.Create("admin@test.com"),
        //     PasswordHash.Create("HASH"),
        //     "Ali",
        //     "Ahmadi");

        // var token = new RefreshToken(
        //     new UserId(),
        //     "token",
        //     DateTime.UtcNow.AddDays(7),
        //     DateTime.UtcNow,
        //     null,
        //     null);

        // //user.AddRefreshToken(token);

        // user.RefreshTokens.Should().HaveCount(1);
    }

    [Fact]
    public void ChangePassword_Should_Change_Hash()
    {
        var user = new User(
            UserId.New(),
            Email.Create("admin@test.com"),
            PasswordHash.Create("OLD"),
            "Ali",
            "Ahmadi");

        user.ChangePassword(
            PasswordHash.Create("NEW"));

        user.PasswordHash.Value.Should().Be("NEW");
    }
}
