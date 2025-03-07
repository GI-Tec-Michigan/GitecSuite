using System.ComponentModel.DataAnnotations;

namespace Gitec.Models.InfoDisplay;

public class InfoBoardItemRssFeed : InfoBoardItem
{
    public InfoBoardItemRssFeed(string title) : base(title)
    {
    }

    [MaxLength(255)]
    public string RssFeedUrl { get; set; } = string.Empty;
}