using BookStore.IdentityService.Application.Contracts.Authentication;
using MediatR;

namespace BookStore.IdentityService.Application.Command.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<LoginResponse>;
