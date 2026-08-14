using BookStore.IdentityService.Application.Command.Login;
using MediatR;

namespace BookStore.IdentityService.Api.Endpoints.Authentication;

public static class LoginEndpoint
{
    public static IEndpointRouteBuilder MapLoginEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login",async (LoginCommand command, ISender sender, CancellationToken cancelationToken)=>
        {
            var response  = await sender.Send(command, cancelationToken);
            return Results.Ok(response);
        }).WithName("Login").WithTags("Users");
        return app;
    }
}
