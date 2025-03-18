using GitecEngine.Enumerations;

namespace GitecEngine.InfoBoard.Models.Boards;

public class ElementalBoard : Board
{
    public ElementalBoard(string title)
    {
        Type = BoardType.Elemental;
    }
}