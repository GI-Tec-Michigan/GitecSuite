using Gitec.GitecBulletin.Enums;

namespace Gitec.GitecBulletin.Models.Elements;

public class ImageElement : Element
{
    public ImageElement(string title) : base(title)
    {
        Type = ElementType.Image;
    }
    public string ImageCaption => Content;
    public string ImageAlt { get; set; } = "";
    public string ImageSrc { get; set; } = "";
    public Justify TextPosition { get; set; } = Justify.Center;
}