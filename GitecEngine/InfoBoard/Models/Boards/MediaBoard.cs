using GitecEngine.Enumerations;

namespace GitecEngine.InfoBoard.Models.Boards;

public class MediaBoard : Board
{
    public MediaBoard(string title)
    {
        Type = BoardType.Media;
    }
    public MediaType MediaType { get; set; } = MediaType.Unknown;
    public string MediaContent { get; set; } = string.Empty;
    public string MediaCaption { get; set; } = string.Empty;
    public string MediaStory { get; set; } = string.Empty;
}