using GitecEngine.Enumerations;

namespace GitecEngine.InfoBoard.Models.Elements;

public class LiveDataElement : Element
{
    public LiveDataElement(string title) : base(title)
    {
        Type = ElementType.LiveData;
    }
    public string LiveDataContent
    {
        get => Content;
        set => Content = value;
    }

    public string LiveDataSrc { get; set; } = "";
    public string LiveDataSubtitle { get; set; } = "";
}