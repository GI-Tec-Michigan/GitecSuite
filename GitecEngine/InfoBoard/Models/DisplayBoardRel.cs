using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GitecEngine.Data;
using GitecEngine.Models;

namespace GitecEngine.InfoBoard.Models;

public class DisplayBoardRel
{
    public DisplayBoardRel(Guid displayOid, Guid boardOid, int orderIndex = 0)
    {
        DisplayOid = displayOid;
        BoardOid = boardOid;
        OrderIndex = orderIndex;
    }

    public DisplayBoardRel() { } // For EF Core

    [Key]
    public int Oid { get; set; }

    // Foreign Keys
    [ForeignKey(nameof(Display))]
    public Guid DisplayOid { get; set; }

    [ForeignKey(nameof(Board))]
    public Guid BoardOid { get; set; }

    // Lazy-loaded Navigation Properties
    public virtual Display Display { get; set; } = null!;
    public virtual Board Board { get; set; } = null!;

    public int OrderIndex { get; set; } = 0;
}