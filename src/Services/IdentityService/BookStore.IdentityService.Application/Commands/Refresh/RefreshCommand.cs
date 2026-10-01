using BookStore.IdentityService.Application.Contracts.Authentication;
using MediatR;

namespace BookStore.IdentityService.Application.Command.Refresh;

public sealed record RefreshCommand(string RefreshToken, string? Device, string? IpAddress) : IRequest<LoginResponse>;
