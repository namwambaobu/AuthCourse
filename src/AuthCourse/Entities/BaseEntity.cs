using System.ComponentModel.DataAnnotations;

namespace AuthCourse.Entities;

public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    public string CreatedBy { get; set; } = "SYS";

    public DateTime CreatedOn { get; set; }

    public string ModifiedBy { get; set; } = "SYS";

    public DateTime ModifiedOn { get; set; }
}