// Features/Users/GetAllUsers/GetAllUsersEndpoint.cs
using AuthCourse.Abstractions;
using AuthCourse.Constants;
using AuthCourse.Database;
using Microsoft.EntityFrameworkCore;

namespace AuthCourse.Features.Users.GetAllUsers;

public static class GetAllUsersEndpoint
{
    public static void MapGetAllUsersEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/users", async (AppDbContext context, CancellationToken ct) =>
            {
                var users = await context.Users
                    .AsNoTracking()
                    .Include(u => u.Roles)
                    .Select(u => new
                    {
                        u.Id,
                        u.Email,
                        u.FirstName,
                        u.LastName,
                        u.IsActive,
                        Roles = u.Roles.Select(r => r.Name)
                    })
                    .ToListAsync(ct);

                return Results.Ok(users);
            })
            .WithName("GetAllUsers")
            .WithTags("Users")
            .RequireAuthorization(Policies.ReadUsers)    // ← users:read permission
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .WithSummary("Get all users")
            .WithDescription("Requires users:read permission. Admin, Manager, SuperAdmin.");
    }
}