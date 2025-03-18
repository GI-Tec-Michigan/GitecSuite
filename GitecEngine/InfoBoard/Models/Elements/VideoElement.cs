using GitecEngine.Enumerations;

namespace GitecEngine.InfoBoard.Models.Elements;

public class VideoElement : Element
{
    public VideoElement(string title) : base(title)
    {
        Type = ElementType.Video;
    }
    public string VideoCaption
    {
        get => Content;
        set => Content = value;
    }

    public string VideoSrc { get; set; } = "";
    public Justify TextPosition { get; set; } = Justify.Center;

}
