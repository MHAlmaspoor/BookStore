using BookStore.IdentityService.Application.Command.Refresh;
using MediatR;

namespace BookStore.IdentityService.Api.Endpoints.Authentication;

public static class RefreshEndpoint
{
    public static IEndpointRouteBuilder MapRefreshEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/refresh", async (RefreshCommand command, ISender sender, CancellationToken cancelationToken)=>
        {
            var responce = await sender.Send(command, cancelationToken);

            return Results.Ok(responce);
        });

        return app;
    }
}
