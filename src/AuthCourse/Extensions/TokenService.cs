// Extensions/TokenService.cs
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthCourse.Abstractions;
using AuthCourse.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthCourse.Extensions;

public sealed class TokenService(IOptions<JwtOptions> options) : ITokenService
{
    private readonly JwtOptions _jwt = options.Value;

    public string GenerateToken(User user) 
    {
        var claims = BuildClaims(user);
        var signingCredentials = BuildSigningCredentials();
        var token = BuildToken(claims, signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // ── Private helpers ──────────────────────────────────────────────────────

    private static List<Claim> BuildClaims(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub,   user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),
            new(ClaimTypes.GivenName,          user.FirstName),
            new(ClaimTypes.Surname,            user.LastName),
        };

        // One claim per role
        foreach (var role in user.Roles)
            claims.Add(new Claim(ClaimTypes.Role, role.Name));

        // One claim per unique permission (deduplicated across roles)
        var permissions = user.Roles
            .SelectMany(r => r.Permissions)
            .Select(p => p.Name)
            .Distinct();

        foreach (var permission in permissions)
            claims.Add(new Claim("permission", permission));

        return claims;
    }

    private SigningCredentials BuildSigningCredentials()
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwt.SecretKey));

        return new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);
    }

    private JwtSecurityToken BuildToken(
        List<Claim> claims,
        SigningCredentials signingCredentials)
        => new(
            issuer:             _jwt.Issuer,
            audience:           _jwt.Audience,
            claims:             claims,
            notBefore:          DateTime.UtcNow,
            expires:            DateTime.UtcNow.AddMinutes(_jwt.ExpiryMinutes),
            signingCredentials: signingCredentials);
}