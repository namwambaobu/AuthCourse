// Features/Auth/Register/RegisterEndpoint.cs
using AuthCourse.Abstractions;
using MediatR;

namespace AuthCourse.Features.Auth.Register;

public static class RegisterEndpoint
{
    public static void MapRegisterEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/register", async (
                RegisterCommand command,
                ISender sender,
                CancellationToken ct) =>
            {
                var result = await sender.Send(command, ct);

                return result.IsSuccess
                    ? Results.Created($"/users/{result.Value!.UserId}", result.Value)
                    : Results.BadRequest(result.Error);
            })
            .WithName("Register")
            .WithTags("Auth")
            .Produces<RegisterResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict)
            .WithSummary("Register a new user")
            .WithDescription("Creates a new user account with the specified role.");
    }
}