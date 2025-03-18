using GitecEngine.Enumerations;

namespace GitecEngine.InfoBoard.Models.Elements;

public class ImageElement : Element
{
    public ImageElement(string title) : base(title)
    {
        Type = ElementType.Image;
    }
    public string ImageCaption
    {
        get => Content;
        set => Content = value;
    }

    public string ImageAlt { get; set; } = "";
    public string ImageSrc { get; set; } = "";
    public Justify TextPosition { get; set; } = Justify.Left;
}