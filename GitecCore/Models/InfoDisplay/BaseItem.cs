using System.ComponentModel.DataAnnotations;

namespace Gitec.Models.InfoDisplay;

public class BaseItem
{
    protected BaseItem()
    {

    }

    [Key]
    public Guid Uid { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public bool IsDeleted { get; set; } = false;
    public bool IsPublished { get; set; } = true;


    public void Delete()
    {
        IsDeleted = true;
    }

    public void Publish()
    {
        IsPublished = true;
    }

    public void UnPublish()
    {
        IsPublished = false;
    }

    public void Restore()
    {
        IsDeleted = false;
    }
}