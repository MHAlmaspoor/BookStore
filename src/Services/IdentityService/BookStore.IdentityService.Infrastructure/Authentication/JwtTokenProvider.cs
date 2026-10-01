using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookStore.IdentityService.Application.Abstraction.Authentication;
using BookStore.IdentityService.Application.Contracts.Authentication;
using BookStore.IdentityService.Domain.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BookStore.IdentityService.Infrastructure.Authentication;

public sealed class JwtTokenProvider : ITokenProvider
{
    private readonly JwtOptions _jwOption;

    public JwtTokenProvider(IOptions<JwtOptions> jwOption)
    {
        _jwOption=jwOption.Value;
    }

    public LoginResponse CreateAccessToken(User user, IReadOnlyCollection<string> permissions)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwOption.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email.Value),
            new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach(var permission in permissions)
            claims.Add(new Claim("permission", permission));

        var token = new JwtSecurityToken(
            issuer: _jwOption.Issuer,
            audience: _jwOption.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwOption.ExpirationMinutes),
            signingCredentials: credentials);

        var expires = DateTime.UtcNow.AddMinutes(_jwOption.ExpirationMinutes);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return new LoginResponse(user.Id.Value, accessToken, null, expires);
        }
}
