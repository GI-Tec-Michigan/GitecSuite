using System.ComponentModel.DataAnnotations;

namespace Gitec.Models.GTBulletin;

public class InfoBoard : BaseEntity
{
    [MaxLength(50)]
    public string Title { get; set; } = string.Empty;
    public int SortOrder { get; set; } = 0;
    public bool IsPublished { get; set; } = false;
    public bool IsArchived { get; set; } = false;
    
    public ICollection<InfoBoardItem> InfoBoardItems { get; set; } = new List<InfoBoardItem>();
}

public class InfoBoardItem : BaseEntity
{
    [MaxLength(50)]
    public string Title { get; set; } = string.Empty;
    public string JsonContent { get; set; } = string.Empty;
    public int SortOrder { get; set; } = 0;
    public bool IsPublished { get; set; } = false;
    public bool IsArchived { get; set; } = false;
    public InfoBoardItemType Type { get; set; } = InfoBoardItemType.Text;
}

public enum InfoBoardItemType
{
    Text,
    Image,
    Video,
    Markdown,
    File,
    RssFeed
}

public class InfoBoardItemText : InfoBoardItem
{
    public string Content { get; set; } = string.Empty;
}

public class InfoBoardItemImage : InfoBoardItem
{
    [MaxLength(255)]
    public string ImageUrl { get; set; } = string.Empty;
    [MaxLength(255)]
    public string ImageAlt { get; set; } = string.Empty;
    [MaxLength(500)]
    public string ImageCaption { get; set; } = string.Empty;
}

public class InfoBoardItemVideo : InfoBoardItem
{
    [MaxLength(255)]
    public string VideoSource { get; set; } = string.Empty;
    [MaxLength(255)]
    public string VideoAlt { get; set; } = string.Empty;
    [MaxLength(500)]
    public string VideoCaption { get; set; } = string.Empty;
}

public class InfoBoardItemMarkdown : InfoBoardItem
{
    public string MarkdownContent { get; set; } = string.Empty;
}

public class InfoBoardItemRssFeed : InfoBoardItem
{
    [MaxLength(255)]
    public string RssFeedUrl { get; set; } = string.Empty;
}