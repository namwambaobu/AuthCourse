using AuthCourse.Pipelines;
using FluentValidation;
using MediatR;

namespace AuthCourse.Extensions;

public static class MediatRExtensions
{
    public static IServiceCollection AddMediatRWithBehaviours(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        services.AddValidatorsFromAssembly(
            typeof(Program).Assembly,
            includeInternalTypes: true);

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(LoggingBehaviour<,>));

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehaviour<,>));

        return services;
    }
}