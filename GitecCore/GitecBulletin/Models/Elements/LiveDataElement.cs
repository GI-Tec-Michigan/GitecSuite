using Gitec.GitecBulletin.Enums;

namespace Gitec.GitecBulletin.Models.Elements;

public class LiveDataElement : Element
{
    public LiveDataElement(string title) : base(title)
    {
        Type = ElementType.LiveData;
    }
    public string LiveDataContent => Content;
    public string LiveDataSrc { get; set; } = "";
    public string LiveDataSubtitle { get; set; } = "";
}