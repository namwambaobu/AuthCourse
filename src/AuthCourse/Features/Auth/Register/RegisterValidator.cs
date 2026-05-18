using AuthCourse.Constants;
using FluentValidation;

namespace AuthCourse.Features.Auth.Register;

public class RegisterValidator : AbstractValidator<RegisterCommand>
{
    private static readonly string[] ValidRoleNames =
    [
        RoleNames.SuperAdmin,
        RoleNames.Admin,
        RoleNames.Manager,
        RoleNames.User,
        RoleNames.Guest
    ];

    public RegisterValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid address.")
            .MaximumLength(256).WithMessage("Email must not exceed 256 characters.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters.");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role is required.")
            .Must(r => ValidRoleNames.Contains(r))
            .WithMessage($"Role must be one of: {string.Join(", ", ValidRoleNames)}.");
    }
}