namespace AuthCourse.Constants;

public static class Policies
{
    // User management policies
    public const string ReadUsers   = PermissionNames.UsersRead;
    public const string WriteUsers  = PermissionNames.UsersWrite;
    public const string DeleteUsers = PermissionNames.UsersDelete;

    // Role management policies
    public const string ReadRoles   = PermissionNames.RolesRead;
    public const string WriteRoles  = PermissionNames.RolesWrite;

    // Content policies
    public const string ReadContent  = PermissionNames.ContentRead;
    public const string WriteContent = PermissionNames.ContentWrite;
}