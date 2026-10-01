using BookStore.IdentityService.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace BookStore.IdentityService.Domain.Tests.ValueObjects;

public class EmailTests
{
    [Fact]
    public void Create_Should_Create_Email()
    {
        var email = Email.Create("Admin@Test.COM");

        email.Value.Should().Be("admin@test.com");
    }
}
