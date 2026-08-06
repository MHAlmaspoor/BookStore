using MediatR;

namespace BookStore.IdentityService.Application.Command.Logout;

public sealed record LogoutCommand(string RefreshToken) : IRequest;
