namespace Gitec.GitecBulletin.Models.UtilityModels;

public class TimeWindow
{
    public TimeWindow(TimeSpan startTime, TimeSpan endTime)
    {
        StartTime = startTime;
        EndTime = endTime;
    }

    public TimeWindow()
    {
        StartTime = TimeSpan.Zero;
        EndTime = TimeSpan.FromHours(24); // Default to a full day
    }

    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public bool IsWithinWindow(TimeSpan currentTime)
    {
        return currentTime >= StartTime && currentTime <= EndTime;
    }
}