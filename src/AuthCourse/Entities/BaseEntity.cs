using System.ComponentModel.DataAnnotations;

namespace AuthCourse.Entities;

public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();
    public string CratedBy { get; init; } = "SYS";
    public DateTime CreatedOn { get; init; } = DateTime.UtcNow;
    public string ModifiedBy { get; init; } = "SYS";
    public DateTime ModifiedOn { get; init; } = DateTime.UtcNow;
}