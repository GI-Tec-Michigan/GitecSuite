using System.Globalization;
using GitecEngine.Enumerations;

namespace GitecEngine.InfoBoard.Models.Elements;

public class DateTimeElement : HtmlElement
{
    public DateTimeElement(string title) : base(title)
    {
        Type = ElementType.DateTime;
        Content = "<div class=\"full-width-text\">" + DateTime.UtcNow.ToLocalTime().ToString("F") + "</div>";
        CssStyle = ".full-width-text { display: flex; justify-content: space-between; width: 100%; font-size: 1.2em; }";
    }
    public DateTime DateTimeContent
    {
        get => Convert.ToDateTime(Content);
        set => Content = value.ToString(CultureInfo.CurrentCulture);
    }
}