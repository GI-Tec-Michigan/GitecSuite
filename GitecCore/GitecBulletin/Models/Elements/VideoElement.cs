using Gitec.GitecBulletin.Enums;

namespace Gitec.GitecBulletin.Models.Elements;

public class VideoElement : Element
{
    public VideoElement(string title) : base(title)
    {
        Type = ElementType.Video;
    }
    public string VideoCaption => Content;
    public string VideoSrc { get; set; } = "";
    public Justify TextPosition { get; set; } = Justify.Center;

}

public class PredefinedElement : Element
{
    public PredefinedElement(string title) : base(title)
    {
        Type = ElementType.PreDefined;
    }

    public string PredefinedContent => Content;
}

public class WeatherElement : PredefinedElement
{
    public WeatherElement(string title) : base(title)
    {
        
    }
}

public class DateTimeElement : PredefinedElement
{
    public DateTimeElement(string title) : base(title)
    {
            
    }
}