using BookStore.IdentityService.Domain.Common;

namespace BookStore.IdentityService.Domain.ValueObjects;

public readonly record struct RefreshTokenId(Guid Value)
{
    public static RefreshTokenId New() => new(Guid.CreateVersion7());

    public override string ToString() => Value.ToString();
}
