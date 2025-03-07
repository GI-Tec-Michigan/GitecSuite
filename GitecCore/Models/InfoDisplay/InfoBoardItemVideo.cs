using System.ComponentModel.DataAnnotations;

namespace Gitec.Models.InfoDisplay;

public class InfoBoardItemVideo : InfoBoardItem
{
    public InfoBoardItemVideo(string title) : base(title)
    {
    }

    [MaxLength(255)]
    public string VideoSource { get; set; } = string.Empty;
    [MaxLength(255)]
    public string VideoAlt { get; set; } = string.Empty;
    [MaxLength(500)]
    public string VideoCaption { get; set; } = string.Empty;
    public string VideoStory { get; set; } = string.Empty;
}