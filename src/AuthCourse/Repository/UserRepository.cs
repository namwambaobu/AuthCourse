// Repository/UserRepository.cs
using AuthCourse.Abstractions;
using AuthCourse.Database;
using AuthCourse.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthCourse.Repository;

public class UserRepository(AppDbContext context)
    : Repository<User>(context), IUserRepository
{
    public async Task<User?> GetByEmailAsync(
        string email,
        CancellationToken ct = default)
        => await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email.ToLower(), ct);

    public async Task<User?> GetByEmailWithRolesAsync(
        string email,
        CancellationToken ct = default)
        => await DbSet
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .FirstOrDefaultAsync(u => u.Email == email.ToLower(), ct);

    public async Task<User?> GetByIdWithRolesAsync(
        Guid id,
        CancellationToken ct = default)
        => await DbSet
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .FirstOrDefaultAsync(u => u.Id == id, ct);

    public async Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken ct = default)
        => await DbSet
            .AnyAsync(u => u.Email == email.ToLower(), ct);
}