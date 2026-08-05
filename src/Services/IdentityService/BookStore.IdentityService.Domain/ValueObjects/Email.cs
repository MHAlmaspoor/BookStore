using System.Net.Mail;
using BookStore.IdentityService.Domain.Exceptions;

namespace BookStore.IdentityService.Domain.ValueObjects;

public sealed class Email : IEquatable<Email>
{
    public string Value { get; private set;}=null;

    private Email()
    {

    }
    private Email(string value)
    {
        Value=value;
    }
public static Email Create(string value)
{
    ArgumentException.ThrowIfNullOrWhiteSpace(value);

    value = value.Trim().ToLowerInvariant();

    try
    {
        _ = new MailAddress(value);
    }
    catch
    {
        throw new DomainException("Invalid Email address");
    }

    return new Email(value);
}

    public bool Equals(Email? other)
        =>other is not null && Value == other.Value;

    public override bool Equals(object? obj)
        =>Equals(obj as Email);

    public override int GetHashCode()
        => Value.GetHashCode();

    public override string ToString()
        => Value;

    public static implicit operator string(Email email)
        =>email.Value;
}
