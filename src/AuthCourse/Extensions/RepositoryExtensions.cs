using AuthCourse.Abstractions;
using AuthCourse.Repository;

namespace AuthCourse.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}