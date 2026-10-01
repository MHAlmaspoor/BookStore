namespace BookStore.IdentityService.Application.Contracts.Authentication;

public sealed record LoginResponse(Guid UserId, string AccessToken,string RefreshToken, DateTime ExpireAt);
