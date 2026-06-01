using AuthCourse.Constants;
using AuthCourse.Database;

namespace AuthCourse.Features.Users.DeleteUser;

public static class DeleteUserEndpoint
{
    public static void MapDeleteUserEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/users/{id:guid}", async (
                Guid id,
                AppDbContext context,
                CancellationToken ct) =>
            {
                var user = await context.Users.FindAsync([id], ct);
                if (user is null) return Results.NotFound();

                context.Users.Remove(user);
                await context.SaveChangesAsync(ct);

                return Results.NoContent();
            })
            .WithName("DeleteUser")
            .WithTags("Users")
            .RequireAuthorization(Policies.DeleteUsers)  // ← users:delete permission
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Delete a user")
            .WithDescription("Requires users:delete permission. SuperAdmin and Admin only.");
    }
}