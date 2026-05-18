// Extensions/DatabaseExtensions.cs
using AuthCourse.Database;
using Microsoft.EntityFrameworkCore;

namespace AuthCourse.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}