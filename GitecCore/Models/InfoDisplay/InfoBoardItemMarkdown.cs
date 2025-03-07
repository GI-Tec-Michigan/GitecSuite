namespace Gitec.Models.InfoDisplay;

public class InfoBoardItemMarkdown : InfoBoardItem
{
    public InfoBoardItemMarkdown(string title) : base(title)
    {
    }

    public string MarkdownContent { get; set; } = string.Empty;
}