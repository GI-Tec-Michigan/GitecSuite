using Gitec.GitecBulletin.Enums;

namespace Gitec.GitecBulletin.Models.Elements;

public class HtmlElement : Element
{
    public HtmlElement(string title) : base(title)
    {
        Type = ElementType.Html;
    }
    public string HtmlContent => Content;
    public string CssStyle { get; set; } = string.Empty;
    public string Javascript { get; set; } = string.Empty;
}