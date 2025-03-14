using Gitec.ExceptionHandling;

namespace Gitec.GitecBulletin.Models;

public class XBoardElements
{
    public XBoardElements(
        Board board, 
        Element element,
        int order = 0)
    {
        Board = board;
        Element = element;
        Order = order;

        if(Order < 0)
        {
            throw new ConfigurationException("Order must be greater than or equal to 0");
        }

    }

    public Guid BulletinBoardId { get; set; }
    public Guid BulletinElementId { get; set; }
    public Board Board { get; set; }
    public Element Element { get; set; }
    public int Order { get; set; } = 0;
}