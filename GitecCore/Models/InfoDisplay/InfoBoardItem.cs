using System.ComponentModel.DataAnnotations;
using Gitec.Data;
using Gitec.Utilities;

namespace Gitec.Models.InfoDisplay;

public abstract class InfoBoardItem : BaseEntity
{
    public InfoBoardItem(string title)
    {
        Title = title;
        Name = title.ToSlug();
    }
    [MaxLength(50)]
    public string Title { get; set; } = string.Empty;
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    public InfoBoardItemType Type { get; set; } = InfoBoardItemType.Text;
    public int SortOrder { get; set; } = 0;
    public bool IsPublished { get; set; } = true;
    public bool IsArchived { get; set; } = false;
}