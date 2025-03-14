using Gitec.GitecBulletin.Enums;
using Gitec.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ElementType = Gitec.GitecBulletin.Enums.ElementType;

namespace Gitec.GitecBulletin.Models;

public class Element : EntityBase
{
    public Element(string title) : base(title)
    {
    }
    
    public string Content { get; set; } = "";
    public ElementType Type { get; set; } = ElementType.Unknown;
    public SchedulePackage Schedule { get; set; } = new SchedulePackage();
    public ThemeDef Theme { get; set; } = new ThemeDef("Default");
    public ICollection<XBoardElements> BulletinBoardElements { get; set; } = new List<XBoardElements>();
}