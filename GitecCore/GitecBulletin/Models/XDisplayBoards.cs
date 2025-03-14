using Gitec.ExceptionHandling;

namespace Gitec.GitecBulletin.Models;

public class XDisplayBoards
{
    public XDisplayBoards(
        DisplayScreen display, 
        Board board,
        int order = 0)
    {
        DisplayScreen = display;
        Board = board;
        Order = order;

        if (Order < 0)
        {
            throw new ConfigurationException("Order must be greater than or equal to 0");
        }
    }

    public Guid BulletinDisplayId { get; set; }
    public Guid BulletinBoardId { get; set; }
    public DisplayScreen DisplayScreen { get; set; }
    public Board Board { get; set; }
    public int Order { get; set; } = 0;
}
