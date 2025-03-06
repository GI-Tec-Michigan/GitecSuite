using System.ComponentModel.DataAnnotations;
using Gitec.Data;
using Gitec.Utilities;

namespace Gitec.Models.InfoDisplay;

public class InfoBoard : BaseEntity
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



public class InfoBoardItemText : InfoBoardItem
{
    public InfoBoardItemText(string title) : base(title)
    {
    }

    public string TextContent { get; set; } = string.Empty;
}

public class InfoBoardItemImage : InfoBoardItem
{
    public InfoBoardItemImage(string title) : base(title)
    {
    }

    [MaxLength(255)]
    public string ImageUrl { get; set; } = string.Empty;
    [MaxLength(255)]
    public string ImageAlt { get; set; } = string.Empty;
    [MaxLength(500)]
    public string ImageCaption { get; set; } = string.Empty;
    public string ImageStory { get; set; } = string.Empty;
}

public class InfoBoardItemVideo : InfoBoardItem
{
    public InfoBoardItemVideo(string title) : base(title)
    {
    }

    [MaxLength(255)]
    public string VideoSource { get; set; } = string.Empty;
    [MaxLength(255)]
    public string VideoAlt { get; set; } = string.Empty;
    [MaxLength(500)]
    public string VideoCaption { get; set; } = string.Empty;
    public string VideoStory { get; set; } = string.Empty;
}

public class InfoBoardItemMarkdown : InfoBoardItem
{
    public InfoBoardItemMarkdown(string title) : base(title)
    {
    }

    public string MarkdownContent { get; set; } = string.Empty;
}

public class InfoBoardItemRssFeed : InfoBoardItem
{
    public InfoBoardItemRssFeed(string title) : base(title)
    {
    }

    [MaxLength(255)]
    public string RssFeedUrl { get; set; } = string.Empty;
}

public class InfoBoardItemApiData : InfoBoardItem
{
    public InfoBoardItemApiData(string title) : base(title)
    {
    }
    public string ApiUrl { get; set; } = string.Empty;
    public string ApiData { get; set; } = string.Empty;
}