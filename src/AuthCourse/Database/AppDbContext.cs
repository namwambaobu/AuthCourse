using AuthCourse.Constants;
using AuthCourse.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthCourse.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User>       Users       => Set<User>();
    public DbSet<Role>       Roles       => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── User ──────────────────────────────────────────────────────────
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.Id);
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.Email).IsRequired().HasMaxLength(256);
            e.Property(u => u.PasswordHash).IsRequired();
            e.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
            e.Property(u => u.LastName).IsRequired().HasMaxLength(100);
        });

        // ── Role ──────────────────────────────────────────────────────────
        modelBuilder.Entity<Role>(e =>
        {
            e.HasKey(r => r.Id);
            e.HasIndex(r => r.Name).IsUnique();
            e.Property(r => r.Name).IsRequired().HasMaxLength(50);
            e.Property(r => r.Description).HasMaxLength(200);
        });

        // ── Permission ────────────────────────────────────────────────────
        modelBuilder.Entity<Permission>(e =>
        {
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Name).IsUnique();
            e.Property(p => p.Name).IsRequired().HasMaxLength(100);
            e.Property(p => p.Description).HasMaxLength(200);
        });

        // ── User ↔ Role (many-to-many) ────────────────────────────────────
        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity("UserRoles");

        // ── Role ↔ Permission (many-to-many) ──────────────────────────────
        modelBuilder.Entity<Role>()
            .HasMany(r => r.Permissions)
            .WithMany(p => p.Roles)
            .UsingEntity("RolePermissions"); 

        // ── Seed data ─────────────────────────────────────────────────────
        SeedData(modelBuilder); 
        Console.WriteLine("connected to the database");

    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // Fixed GUIDs so migrations are deterministic
        var superAdminRoleId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var adminRoleId      = Guid.Parse("00000000-0000-0000-0000-000000000002");
        var managerRoleId    = Guid.Parse("00000000-0000-0000-0000-000000000003");
        var userRoleId       = Guid.Parse("00000000-0000-0000-0000-000000000004");
        var guestRoleId      = Guid.Parse("00000000-0000-0000-0000-000000000005");

        var permUsersRead   = Guid.Parse("00000000-0000-0000-0001-000000000001");
        var permUsersWrite  = Guid.Parse("00000000-0000-0000-0001-000000000002");
        var permUsersDelete = Guid.Parse("00000000-0000-0000-0001-000000000003");
        var permRolesRead   = Guid.Parse("00000000-0000-0000-0001-000000000004");
        var permRolesWrite  = Guid.Parse("00000000-0000-0000-0001-000000000005");
        var permContentRead  = Guid.Parse("00000000-0000-0000-0001-000000000006");
        var permContentWrite = Guid.Parse("00000000-0000-0000-0001-000000000007");

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = superAdminRoleId, Name = RoleNames.SuperAdmin, Description = "Full system access" },
            new Role { Id = adminRoleId,      Name = RoleNames.Admin,      Description = "User and content management" },
            new Role { Id = managerRoleId,    Name = RoleNames.Manager,    Description = "Team and content oversight" },
            new Role { Id = userRoleId,       Name = RoleNames.User,       Description = "Standard user access" },
            new Role { Id = guestRoleId,      Name = RoleNames.Guest,      Description = "Read-only guest access" }
        );

        modelBuilder.Entity<Permission>().HasData(
            new Permission { Id = permUsersRead,   Name = PermissionNames.UsersRead,   Description = "Read user records" },
            new Permission { Id = permUsersWrite,  Name = PermissionNames.UsersWrite,  Description = "Create and update users" },
            new Permission { Id = permUsersDelete, Name = PermissionNames.UsersDelete, Description = "Delete users" },
            new Permission { Id = permRolesRead,   Name = PermissionNames.RolesRead,   Description = "Read roles" },
            new Permission { Id = permRolesWrite,  Name = PermissionNames.RolesWrite,  Description = "Create and update roles" },
            new Permission { Id = permContentRead,  Name = PermissionNames.ContentRead,  Description = "Read content" },
            new Permission { Id = permContentWrite, Name = PermissionNames.ContentWrite, Description = "Create and update content" }
        );

        // Role → Permission assignments
        modelBuilder.Entity("RolePermissions").HasData(
            // SuperAdmin gets everything
            new { RolesId = superAdminRoleId, PermissionsId = permUsersRead },
            new { RolesId = superAdminRoleId, PermissionsId = permUsersWrite },
            new { RolesId = superAdminRoleId, PermissionsId = permUsersDelete },
            new { RolesId = superAdminRoleId, PermissionsId = permRolesRead },
            new { RolesId = superAdminRoleId, PermissionsId = permRolesWrite },
            new { RolesId = superAdminRoleId, PermissionsId = permContentRead },
            new { RolesId = superAdminRoleId, PermissionsId = permContentWrite },

            // Admin: user management + content, no role management
            new { RolesId = adminRoleId, PermissionsId = permUsersRead },
            new { RolesId = adminRoleId, PermissionsId = permUsersWrite },
            new { RolesId = adminRoleId, PermissionsId = permUsersDelete },
            new { RolesId = adminRoleId, PermissionsId = permRolesRead },
            new { RolesId = adminRoleId, PermissionsId = permContentRead },
            new { RolesId = adminRoleId, PermissionsId = permContentWrite },

            // Manager: content + read users
            new { RolesId = managerRoleId, PermissionsId = permUsersRead },
            new { RolesId = managerRoleId, PermissionsId = permContentRead },
            new { RolesId = managerRoleId, PermissionsId = permContentWrite },

            // User: own content
            new { RolesId = userRoleId, PermissionsId = permContentRead },
            new { RolesId = userRoleId, PermissionsId = permContentWrite },

            // Guest: read only
            new { RolesId = guestRoleId, PermissionsId = permContentRead }
        );
    }
}