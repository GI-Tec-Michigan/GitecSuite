using System.ComponentModel.DataAnnotations;

namespace Gitec.Models;

public class BaseEntity<T>
{
    protected BaseEntity()
    {

    }

    [Key]
    public T Uid { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

}