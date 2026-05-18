using AuthCourse.Abstractions;
using MediatR;

namespace AuthCourse.Features.Auth.Register;

public record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string Role) : IRequest<Result<RegisterResponse>>;

public record RegisterResponse(Guid UserId, string Email);