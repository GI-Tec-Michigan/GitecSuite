using GitecEngine.Enumerations;
using GitecEngine.Models;

namespace GitecEngine.InfoBoard.Models;

public abstract class Board : BaseEntity
{
    protected Board(string title, BoardType type)
    {
        Title = title;
        Type = type;
        IsActive = false;
    }

    protected Board() { } // for EF Core
    public BoardType Type { get; set; } = BoardType.Unknown;
    public List<SchedulePackage>? SchedulePackages { get; set; } = [];
    
    // Lazy-loaded Navigation Properties
    public virtual ICollection<DisplayBoardRel> DisplayBoardRelations { get; set; } = new HashSet<DisplayBoardRel>();
    public virtual ICollection<BoardElementRel> BoardElementRelations { get; set; } = new HashSet<BoardElementRel>();
}