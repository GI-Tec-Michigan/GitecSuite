namespace Gitec.GitecBulletin.Models.UtilityModels;

public class TimeWindow
{
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public bool IsWithinWindow(TimeSpan currentTime)
    {
        return currentTime >= StartTime && currentTime <= EndTime;
    }
}