using System.Drawing;
using Gitec.Models;

namespace Gitec.GitecBulletin.Models;

public class DisplayTheme : EntityBase
{
    public DisplayTheme(string title) : base(title)
    {
    }
    public bool IsDefault { get; set; } = false;
    public Color BgColor { get; init; } = Color.LightGray;
    public Color TextColor { get; init; } = Color.DarkGray;
    public Color AccentColor { get; init; } = Color.Blue;
    public Color FrameColor { get; init; } = Color.Black;
    public Color FrameAccentColor { get; init; } = Color.White;
    public bool ShowFrame { get; init; } = true;
}
