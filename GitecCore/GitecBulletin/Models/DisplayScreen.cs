using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Gitec.Models;

namespace Gitec.GitecBulletin.Models;

public class DisplayScreen : EntityBase
{
    public DisplayScreen(string title) : base(title)
    {
        
    }

    [MaxLength(50)]
    public required string Location { get; set; }
    [MaxLength(100)]
    public string Description { get; set; } = "";
    public IPAddress HostIp { get; set; } = IPAddress.Parse("127.0.0.1");
    public ICollection<Board> Boards { get; set; } = new List<Board>();
}

