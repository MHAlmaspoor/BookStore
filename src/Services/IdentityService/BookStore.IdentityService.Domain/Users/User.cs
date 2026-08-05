using BookStore.IdentityService.Domain.Common;
using BookStore.IdentityService.Domain.Exceptions;
using BookStore.IdentityService.Domain.Users.Events;
using BookStore.IdentityService.Domain.ValueObjects;
using BookStore.IdentityService.Domain.RefreshTokens;

namespace BookStore.IdentityService.Domain.Users;

public sealed class User : AggregatedRoot
{
    public UserId Id { get; private set; }
    public Email Email { get; private set; }
    public PasswordHash PasswordHash { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public bool IsActive { get; private set; }

    private readonly List<RefreshToken> _refreshTokens=[];

    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

    public User()
    {

    }

public User(UserId id, Email email, PasswordHash passwordHash, string firstName, string lastName)
{
    Id = id;
    Email = email;
    PasswordHash = passwordHash;

    FirstName = firstName.Trim();
    LastName = lastName.Trim();

    IsActive = true;

    AddDomainEvent(new UserRegisteredEvent(Id,Email.Value));
}

    public void ChangeName(string firstName, string lastName)
    {
        ValidateName(firstName, nameof(FirstName));
        ValidateName(lastName, nameof(LastName));

        firstName = firstName.Trim();
        lastName = lastName.Trim();

        if (FirstName == firstName && LastName == lastName)
            return;

        FirstName = firstName;
        LastName = lastName;
    }

    private static void ValidateName(string value, string fieldName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        if (value.Length > 100)
            throw new DomainException($"{fieldName} is too long.");
    }

    public void ChangePassword(PasswordHash passwordHash)
    {
        ArgumentNullException.ThrowIfNull(passwordHash);

        if (PasswordHash == passwordHash)
            return;

        PasswordHash = passwordHash;
        AddDomainEvent(new PasswordChangedEvent(Id));
    }
    public void Deactivate()
    {
        if (!IsActive)
            return;

        IsActive = false;
        AddDomainEvent(new UserDeactivatedEvent(Id));

    }

    public void Activate()
    {
        if (IsActive)
            return;

        IsActive = true;
        AddDomainEvent(new UserActivatedEvent(Id));

    }

    public void AddRefreshToken(string token, DateTime expiresOnUtc, string? device, string? ipAddress)
    {
        _refreshTokens.Add( RefreshToken.Create(
            Id, token, expiresOnUtc, device,ipAddress
        ));
    }
    public static User Register(string firstName, string lastName, Email email, PasswordHash passwordHash)
    {
        return new User(UserId.New(), email, passwordHash, firstName, lastName);
    }

}
