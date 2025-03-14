using Gitec.GitecBulletin.Data;
using Gitec.GitecBulletin.Enums;

namespace Gitec.GitecBulletin.Models.Boards;

public class AnnounceBoard : ElementalBoard
{
    public AnnounceBoard(string title) : base(title)
    {
        BoardType = BoardType.Announcement;
    }
}