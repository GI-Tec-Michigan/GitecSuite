using System.ComponentModel.DataAnnotations;
using Gitec.Utilities;

namespace Gitec.Models;

public class EntityBase
{
    public EntityBase(string title)
    {
        Title = title;
        Name = Title.ToSlug();
    }
    
    public EntityBase()
    {
        // Default constructor for EF Core
    }

    [Key]
    public Guid Uid { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public bool IsArchived { get; set; } = false;
    [MaxLength(100)]
    public string Title { get; set; }
    [MaxLength(100)]
    public string Name { get; set; }

}





