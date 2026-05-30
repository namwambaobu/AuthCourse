using MediatR;

namespace AuthCourse.Features.Auth.Login;

public static class LoginEndpoint
{
    public static void MapLoginEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/login", async (
                LoginCommand command,
                ISender sender,
                CancellationToken ct) =>
            {
                var result = await sender.Send(command, ct);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.Unauthorized();
            })
            .WithName("Login")
            .WithTags("Auth")
            .Produces<LoginResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithSummary("Login and receive a JWT")
            .WithDescription(
                "Validates credentials and returns a signed JWT containing " +
                "role and permission claims.");
    }
}