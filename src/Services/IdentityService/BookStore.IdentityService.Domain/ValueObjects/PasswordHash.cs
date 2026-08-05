namespace BookStore.IdentityService.Domain.ValueObjects;

public sealed class PasswordHash : IEquatable<PasswordHash>
{
    public string Value { get; private set; } = null;

    private PasswordHash()
    {

    }

    private PasswordHash(string value)
    {
        Value=value;
    }

    public static PasswordHash Create(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        return new PasswordHash(value);
    }

    public bool Equals(PasswordHash? other)
        => other is not null && Value == other.Value;

    public override bool Equals(object? obj)
        => Equals(obj as PasswordHash);

    public override int GetHashCode()
        => Value.GetHashCode();

    public override string ToString()
        => Value;

    public static implicit operator string(PasswordHash hash)
        => hash.Value;
}
