using Gitec.GitecBulletin.Data;
using Gitec.GitecBulletin.Enums;

namespace Gitec.GitecBulletin.Models.Boards;

public class ElementalBoard : Board
{
    public ElementalBoard(string title) : base(title)
    {
        BoardType = Enums.BoardType.Elemental;
    }
    public List<Element> Elements { get; set; } = [];
}