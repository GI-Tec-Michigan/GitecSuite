using System.ComponentModel.DataAnnotations;
using GitecEngine.Enumerations;
using GitecEngine.Models;

namespace GitecEngine.InfoBoard.Models;

public abstract class Element : BaseEntity
{
    protected Element(string title) : base(title)
    {
        Title = title;
        IsActive = false;
    }

    protected Element() { } // for EF Core
    public ElementType Type { get; set; } = ElementType.Unknown;
    
    [MaxLength(10000)]
    protected internal string Content { get; set; } = string.Empty;
    public List<SchedulePackage>? SchedulePackages { get; set; } = [];
    
    // Lazy-loaded Navigation Property
    public virtual ICollection<BoardElementRel> BoardElementRelations { get; set; } = new HashSet<BoardElementRel>();
}