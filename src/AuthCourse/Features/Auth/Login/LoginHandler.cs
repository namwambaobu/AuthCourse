using AuthCourse.Abstractions;
using AuthCourse.Exceptions;
using MediatR;
using Microsoft.Extensions.Options;

namespace AuthCourse.Features.Auth.Login;

internal sealed class LoginHandler(
    IUserRepository userRepository,
    ITokenService tokenService,
    IOptions<JwtOptions> jwtOptions)
    : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly JwtOptions _jwt = jwtOptions.Value;

    public async Task<Result<LoginResponse>> Handle(
        LoginCommand command,
        CancellationToken ct)
    {
        // ── 1. Load user with roles and permissions ───────────────────────
        var user = await userRepository
            .GetByEmailWithRolesAsync(command.Email.ToLower(), ct);

        // ── 2. Verify password ───────────────────────────────────────────
        var passwordHash = user?.PasswordHash ?? BCrypt.Net.BCrypt.HashPassword("dummy");
        var passwordValid = BCrypt.Net.BCrypt.Verify(command.Password, passwordHash);

        if (user is null || !passwordValid)
            throw new InvalidCredentialsException();

        // ── 3. Check account is active ────────────────────────────────────
        if (!user.IsActive)
            throw new AccountDeactivatedException();

        // ── 4. Generate token ─────────────────────────────────────────────
        var token = tokenService.GenerateToken(user);

        var roleNames = user.Roles
            .Select(r => r.Name)
            .ToList();

        return Result<LoginResponse>.Success(new LoginResponse(
            Token:     token,
            Email:     user.Email,
            FirstName: user.FirstName,
            LastName:  user.LastName,
            Roles:     roleNames,
            ExpiresAt: DateTime.UtcNow.AddMinutes(_jwt.ExpiryMinutes)));
    }
}