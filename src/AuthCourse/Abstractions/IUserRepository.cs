using AuthCourse.Entities;

namespace AuthCourse.Abstractions;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);

    Task<User?> GetByEmailWithRolesAsync(string email, CancellationToken ct = default);

    Task<User?> GetByIdWithRolesAsync(Guid id, CancellationToken ct = default);

    Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);
}