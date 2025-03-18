using System.ComponentModel.DataAnnotations;
using GitecEngine.Data;
using GitecEngine.Models;

namespace GitecEngine.InfoBoard.Models;

public sealed class Display : BaseEntity
{
    public Display(string title) : base(title)
    {
        Title = title;
        IsActive = false;
    }

    public Display() { } // for EF Core
    

    // Lazy-loaded Navigation Properties
    public ICollection<DisplayBoardRel> DisplayBoardRelations { get; set; } = new HashSet<DisplayBoardRel>();
}