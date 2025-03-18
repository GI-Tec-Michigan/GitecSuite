using GitecEngine.Enumerations;

namespace GitecEngine.InfoBoard.Models.Boards;

public class AnnounceBoard : Board
{
    public AnnounceBoard(string title)
    {
        Type = BoardType.Announcement;
    }
}