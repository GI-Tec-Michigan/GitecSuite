using System.ComponentModel.DataAnnotations;

namespace Gitec.Models;

public class BaseEntity
{
    protected BaseEntity()
    {
        
    }
    
    [Key]
    public Guid Uid { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    
}