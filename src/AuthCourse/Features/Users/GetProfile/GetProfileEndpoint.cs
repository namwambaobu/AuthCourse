using AuthCourse.Constants;
using System.Security.Claims;

namespace AuthCourse.Features.Users.GetProfile;

public static class GetProfileEndpoint
{
    public static void MapGetProfileEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/users/me", (ClaimsPrincipal user) =>
            {
                var userId    = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                ?? user.FindFirst("sub")?.Value;
                var email     = user.FindFirst(ClaimTypes.Email)?.Value
                                ?? user.FindFirst("email")?.Value;
                var roles     = user.FindAll(ClaimTypes.Role).Select(c => c.Value);
                var permissions = user.FindAll("permission").Select(c => c.Value);

                return Results.Ok(new
                {
                    UserId      = userId,
                    Email       = email,
                    Roles       = roles,
                    Permissions = permissions
                });
            })
            .WithName("GetProfile")
            .WithTags("Users")
            .RequireAuthorization()                      // any authenticated user
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithSummary("Get current user profile")
            .WithDescription("Returns the authenticated user's claims from the JWT.");
    }
}