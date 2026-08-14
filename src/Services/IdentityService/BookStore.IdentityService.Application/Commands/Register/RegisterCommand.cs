using MediatR;

namespace BookStore.IdentityService.Application.Command.Register;

public sealed record RegisterCommand(string Email, string Password, string FirstName, string LastName): IRequest<Guid>;
