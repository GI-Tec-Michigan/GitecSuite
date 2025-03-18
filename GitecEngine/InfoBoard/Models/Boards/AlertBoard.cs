using GitecEngine.Enumerations;

namespace GitecEngine.InfoBoard.Models.Boards;

public class AlertBoard : Board
{
    public AlertBoard(string title) 
    {
        Type = BoardType.Alert;
    }
}