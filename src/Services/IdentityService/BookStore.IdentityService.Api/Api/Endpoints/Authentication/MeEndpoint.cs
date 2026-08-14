

namespace BookStore.IdentityService.Api.Endpoints.Authentication;

public static class MeEndpoint
{
    public static IEndpointRouteBuilder MapMeEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/me",()=>
        {
            return Results.Ok("Authenticated");
        }).RequireAuthorization();
        return app;
    }
}
