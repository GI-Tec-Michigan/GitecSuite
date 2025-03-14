using Gitec.GitecBulletin.Data;
using Gitec.GitecBulletin.Enums;

namespace Gitec.GitecBulletin.Models.Boards;

public class AlertBoard : Board
{
    public AlertBoard(string title) : base(title)
    {
        BoardType = Enums.BoardType.Alert;
    }
}