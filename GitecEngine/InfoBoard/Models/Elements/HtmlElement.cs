using GitecEngine.Enumerations;

namespace GitecEngine.InfoBoard.Models.Elements;

public class HtmlElement : Element
{
    private string _cssStyle = string.Empty;
    private string _javascript = string.Empty;

    public HtmlElement(string title) : base(title)
    {
        Type = ElementType.Html;
    }
    public string HtmlContent
    {
        get => Content;
        set => Content = value;
    }

    public string CssStyle
    {
        get => _cssStyle;
        set => _cssStyle = value;
    }

    public string Javascript
    {
        get => _javascript;
        set => _javascript = value;
    }
}