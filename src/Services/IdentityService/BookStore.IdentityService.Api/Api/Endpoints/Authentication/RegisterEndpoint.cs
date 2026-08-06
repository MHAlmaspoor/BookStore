
using BookStore.IdentityService.Application.Command.Register;

using MediatR;

namespace BookStore.IdentityService.Api.Endpoints.Authentication;

public static class RegisterEndpoint
{
    public static IEndpointRouteBuilder MapRegisterEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost( "/api/users/register", async (RegisterCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            var userId = await sender.Send(command, cancellationToken);
            return Results.Created($"/api/users/{userId}", new {UserId = userId});
        }).WithName("Register").WithTags("Users");

        return app;
    }
}
