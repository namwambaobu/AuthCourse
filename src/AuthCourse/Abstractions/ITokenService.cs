using AuthCourse.Entities;

namespace AuthCourse.Abstractions;

public interface ITokenService
{
    string GenerateToken(User user);
}