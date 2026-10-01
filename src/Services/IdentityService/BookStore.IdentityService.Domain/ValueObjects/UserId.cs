namespace BookStore.IdentityService.Domain.ValueObjects;

public readonly record struct UserId(Guid Value)
{
    public static UserId New() => new(Guid.CreateVersion7());
    public override string ToString() =>Value.ToString();

    public static implicit operator Guid(UserId id) => id.Value;

    public static implicit operator UserId(Guid Value) => new(Value);
}
