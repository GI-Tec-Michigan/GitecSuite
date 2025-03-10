namespace Gitec.Models.InfoDisplay;

public class InfoDisplayConfig
{
    public int TransitionDuration { get; set; } = 2000;
    public int TransitionDelay { get; set; } = 10000;
    public string TransitionEffect { get; set; } = "fade";
    public string InfoDisplayTitle { get; set; } = "Gitec Display";
}