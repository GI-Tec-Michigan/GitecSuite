using System.ComponentModel.DataAnnotations;

namespace Gitec.Models.InfoDisplay;

public class InfoBoardItemImage : InfoBoardItem
{
    public InfoBoardItemImage(string title) : base(title)
    {
    }

    [MaxLength(255)]
    public string ImageUrl { get; set; } = string.Empty;
    [MaxLength(255)]
    public string ImageAlt { get; set; } = string.Empty;
    [MaxLength(500)]
    public string ImageCaption { get; set; } = string.Empty;
    public string ImageStory { get; set; } = string.Empty;
}