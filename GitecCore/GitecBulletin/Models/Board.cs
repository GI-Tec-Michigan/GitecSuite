using Gitec.GitecBulletin.Data;
using Gitec.GitecBulletin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Gitec.GitecBulletin.Enums;
using Gitec.Models;

namespace Gitec.GitecBulletin.Models;

public class Board : EntityBase
{
    public Board(string title) : base(title)
    {
    }
    
    public BoardType BoardType { get; set; } = BoardType.Unknown;
    public SchedulePackage Schedule { get; set; } = new SchedulePackage();
    public ThemeDef Theme { get; set; } = new ThemeDef("Default");
    public ICollection<XBoardElements> BulletinBoardElements { get; set; } = new List<XBoardElements>();
    public ICollection<XDisplayBoards> BulletinDisplayBoards { get; set; } = new List<XDisplayBoards>();
}