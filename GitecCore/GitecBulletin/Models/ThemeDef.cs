using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gitec.Models;

namespace Gitec.GitecBulletin.Models;

public class ThemeDef : EntityBase
{
    public ThemeDef(string title) : base(title)
    {
    }
    public Color BgColor { get; set; } = Color.LightGray;
    public Color TextColor { get; set; } = Color.DarkGray;
    public Color AccentColor { get; set; } = Color.Blue;
    public Color FrameColor { get; set; } = Color.Black;
    public Color FrameAccentColor { get; set; } = Color.White;
}
