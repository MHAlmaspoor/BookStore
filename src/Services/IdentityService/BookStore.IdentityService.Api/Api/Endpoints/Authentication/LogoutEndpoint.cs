using BookStore.IdentityService.Application.Command.Logout;
using MediatR;

namespace BookStore.IdentityService.Api.Endpoints.Authentication;

public static class LogoutEndpoint
{
    public static void MapLogoutEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/logout", async(LogoutCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            await sender.Send(command, cancellationToken);
            return Results.NoContent();
        });
    }
}
