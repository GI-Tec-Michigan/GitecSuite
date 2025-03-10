using System.ComponentModel.DataAnnotations;
using Gitec.Utilities;

namespace Gitec.Models.InfoDisplay;

public class InfoBoard : BaseEntity<Guid>
{
    public InfoBoard(string title)
    {
        Title = title;
        Name = title.ToSlug();
    }

    [MaxLength(50)]
    public string Title { get; set; }
    [MaxLength(50)]
    public string Name { get; set; }
    public int SortOrder { get; set; } = 0;
    public bool IsPublished { get; set; } = true;
    public bool IsArchived { get; set; } = false;

    public ICollection<InfoBoardItem> InfoBoardItems { get; set; } = new List<InfoBoardItem>();
}