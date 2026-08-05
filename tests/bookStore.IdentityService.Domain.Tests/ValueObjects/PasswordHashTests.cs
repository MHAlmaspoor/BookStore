using BookStore.IdentityService.Domain.ValueObjects;
using FluentAssertions;

public class PasswordHashTests
{
    [Fact]
    public void Create_should_create_Hash()
    {
        var hash = PasswordHash.Create("abcdes");
        hash.Value.Should().Be("abcdes");
    }

    [Fact]
    public void Create_Should_throw_when_Empty()
    {
        var action = () => PasswordHash.Create("");
        action.Should().Throw<ArgumentException>();
    }
}
