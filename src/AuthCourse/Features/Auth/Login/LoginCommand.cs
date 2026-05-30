using AuthCourse.Abstractions;
using MediatR;

namespace AuthCourse.Features.Auth.Login;

public record LoginCommand(
    string Email,
    string Password) : IRequest<Result<LoginResponse>>;

public record LoginResponse(
    string Token,
    string Email,
    string FirstName,
    string LastName,
    IEnumerable<string> Roles,
    DateTime ExpiresAt);