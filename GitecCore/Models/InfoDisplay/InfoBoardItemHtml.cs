namespace Gitec.Models.InfoDisplay;

public class InfoBoardItemHtml : InfoBoardItem
{
    public InfoBoardItemHtml(string title) : base(title)
    {
    }

    public string HtmlContent { get; set; } = string.Empty;
}