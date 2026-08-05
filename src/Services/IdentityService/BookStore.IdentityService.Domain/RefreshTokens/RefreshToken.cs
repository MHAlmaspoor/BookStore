using BookStore.IdentityService.Domain.Common;
using BookStore.IdentityService.Domain.Users;
using BookStore.IdentityService.Domain.ValueObjects;

namespace BookStore.IdentityService.Domain.RefreshTokens;

public sealed class RefreshToken : Entity
{
    public UserId UserId { get; private set; }
    public User User { get; private set; } = null;
    public string Token { get; private set; } = null;
    public DateTime ExpiresOnUtc { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? RevokedOnUtc { get; private set; }
    public string? ReplacedByToken { get; private set; }
    public string? Device { get; private set; }
    public string? IpAddress { get; private set; }

    private RefreshToken()
    {

    }

    private RefreshToken(UserId userId, string token, DateTime expiresOnUtc, DateTime createdOnUtc,
        string? device, string? ipAddress)
    {
        UserId = userId;
        Token = token;

        ExpiresOnUtc = expiresOnUtc;
        CreatedOnUtc = createdOnUtc;

        Device = device;
        IpAddress = ipAddress;
    }

    public bool IsExpired
        => DateTime.UtcNow >= ExpiresOnUtc;

    public bool IsRevoked
        => RevokedOnUtc.HasValue;

    public bool IsActive
        => !IsExpired && !IsRevoked;

    public void Revoke(string? replacedByToken = null)
    {
        if(IsRevoked)
            return;
        RevokedOnUtc = DateTime.UtcNow;
        ReplacedByToken = replacedByToken;
    }

    public static RefreshToken Create(UserId userId, string token, DateTime expiresOnUtc, string? device = null,
        string? ipAddress = null)
    {
    return new RefreshToken(userId, token, expiresOnUtc,
        DateTime.UtcNow, device, ipAddress);
    }

    public bool CanBeUsed()
    {
        return !IsExpired && !IsRevoked;
    }
}
