using Microsoft.AspNetCore.Authorization;

namespace AuthCourse.Abstractions;

public sealed class PermissionRequirement(string permission)
    : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}