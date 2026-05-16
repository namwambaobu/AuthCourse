namespace AuthCourse.Entities;

public class Permission : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    //Navigation
    public ICollection<User> Users { get; set; } = [];
    public ICollection<Role> Roles { get; set; } = [];
}