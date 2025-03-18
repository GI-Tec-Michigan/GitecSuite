using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GitecEngine.InfoBoard.Models;

public class BoardElementRel
{
    public BoardElementRel(Guid boardOid, Guid elementOid, int orderIndex = 0)
    {
        BoardOid = boardOid;
        ElementOid = elementOid;
        OrderIndex = orderIndex;
    }

    public BoardElementRel() { } // For EF Core

    [Key]
    public int Oid { get; set; }

    // Foreign Keys
    [ForeignKey(nameof(Board))]
    public Guid BoardOid { get; set; }

    [ForeignKey(nameof(Element))]
    public Guid ElementOid { get; set; }

    // Lazy-loaded Navigation Properties
    public virtual Board Board { get; set; } = null!;
    public virtual Element Element { get; set; } = null!;

    public int OrderIndex { get; set; } = 0;
}