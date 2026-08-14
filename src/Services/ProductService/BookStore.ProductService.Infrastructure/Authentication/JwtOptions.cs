namespace BookStore.ProductService.Infrastructure.Authentication;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string SecretKey { get; init; } = null!;

    public string Issuer { get; init; } = null!;

    public string Audience { get; init; } = null!;
}
