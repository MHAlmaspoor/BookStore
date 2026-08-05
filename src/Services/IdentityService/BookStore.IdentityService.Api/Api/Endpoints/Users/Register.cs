using BookStore.IdentityService.Application.Command.Login;
using BookStore.IdentityService.Application.Command.Refresh;
using BookStore.IdentityService.Application.Command.Register;
using BookStore.IdentityService.Domain.ValueObjects;
using MediatR;

namespace BookStore.IdentityService.Api.Endpoints.Users;

public static class Register
{
    public static IEndpointRouteBuilder MapRegisterEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost( "/api/users/register", async (RegisterCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            var userId = await sender.Send(command, cancellationToken);
            return Results.Created($"/api/users/{userId}", new {UserId = userId});
        }).WithName("Register").WithTags("Users");

        app.MapPost("/api/auth/login",async (LoginCommand command, ISender sender, CancellationToken cancelationToken)=>
        {
            var response  = await sender.Send(command, cancelationToken);
            return Results.Ok(response);
        }).WithName("Login").WithTags("Users");

        app.MapGet("/me",()=>
        {
            return Results.Ok("Authenticated");
        }).RequireAuthorization();

        app.MapPost("api/auth/refresh", async( RefreshCommand command, ISender sender, CancellationToken cancelationToken) =>
        {
            var responce = await sender.Send(command, cancelationToken);
            return Results.Ok(responce);
        });

        return app;
    }
}
