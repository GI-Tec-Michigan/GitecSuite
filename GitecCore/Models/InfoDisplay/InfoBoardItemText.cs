namespace Gitec.Models.InfoDisplay;

public class InfoBoardItemText : InfoBoardItem
{
    public InfoBoardItemText(string title) : base(title)
    {
    }

    public string TextContent { get; set; } = string.Empty;
}