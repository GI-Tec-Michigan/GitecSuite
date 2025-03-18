using System.ComponentModel.DataAnnotations;
using GitecEngine.Utilities;

namespace GitecEngine.Models;

public class BaseEntity
{
    public BaseEntity(string title)
    {
        Title = title;
        Name = Title.ToSlug();
    }
    
    public BaseEntity()
    {
        // Default constructor for EF Core
    }

    [Key]
    public Guid Uid { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = false;
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

}