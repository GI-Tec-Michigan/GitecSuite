using Gitec.GitecBulletin.Enums;

namespace Gitec.GitecBulletin.Models.Elements;

public class MarkdownElement : Element
{
    public MarkdownElement(string title) : base(title)
    {
        Type = ElementType.Markdown;
    }
    public string MarkdownContent => Content;
}