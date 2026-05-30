using AuthCourse.Abstractions;
using AuthCourse.Constants;
using AuthCourse.Database;
using AuthCourse.Entities;
using AuthCourse.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuthCourse.Features.Auth.Register;

internal sealed class RegisterHandler(
    IUserRepository userRepository,
    AppDbContext context)
    : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
{
    public async Task<Result<RegisterResponse>> Handle(
        RegisterCommand command,
        CancellationToken ct)
    {
        // ── 1. Guard: duplicate email ────────────────────────────────────
        var exists = await userRepository.ExistsByEmailAsync(command.Email, ct);
        if (exists)
            throw new DuplicateEmailException(command.Email);

        // ── 2. Load the requested role from the database ─────────────────
        var role = await context.Roles
                       .FirstOrDefaultAsync(r => r.Name == command.Role, ct)
                   ?? throw new InvalidOperationException(
                       $"Role '{command.Role}' not found. Ensure seed data has been applied.");

        // ── 3. Build the user entity ──────────────────────────────────────
        var user = new User
        {
            Email        = command.Email.ToLower(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(command.Password),
            FirstName    = command.FirstName,
            LastName     = command.LastName,
            IsActive     = true,
            Roles        = [role]
        };

        // ── 4. Persist ────────────────────────────────────────────────────
        await userRepository.AddAsync(user, ct);
        await userRepository.SaveChangesAsync(ct);

        // ── 5. Return success ─────────────────────────────────────────────
        return Result<RegisterResponse>.Success(
            new RegisterResponse(user.Id, user.Email));
    }
}