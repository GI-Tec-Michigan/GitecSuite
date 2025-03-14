using Gitec.GitecBulletin.Data;
using Gitec.GitecBulletin.Enums;

namespace Gitec.GitecBulletin.Models.Boards;

public class MediaBoard : Board
{
    public MediaBoard(string title) : base(title)
    {
        BoardType = Enums.BoardType.Media;
    }
    public MediaType MediaType { get; set; } = MediaType.Unknown;
    public string MediaContent { get; set; } = string.Empty;
    public string MediaCaption { get; set; } = string.Empty;
    public string MediaStory { get; set; } = string.Empty;
}