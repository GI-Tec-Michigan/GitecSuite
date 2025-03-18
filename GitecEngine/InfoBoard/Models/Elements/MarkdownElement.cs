using GitecEngine.Enumerations;

namespace GitecEngine.InfoBoard.Models.Elements;

public class MarkdownElement : Element
{
    public MarkdownElement(string title) : base(title)
    {
        Type = ElementType.Markdown;
    }
    public string MarkdownContent
    {
        get => Content;
        set => Content = value;
    }
}