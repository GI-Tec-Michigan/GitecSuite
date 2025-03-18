using GitecEngine.Models;

namespace GitecEngine.InfoBoard.Models;

public class SchedulePackage : BaseEntity
{
    public SchedulePackage(string title) : base(title)
    {
        
    }

    public SchedulePackage() { } // Empty constructor for serialization



    public List<ScheduleDate> Dates { get; set; } = new();
    public List<ScheduleTime> Times { get; set; } = new();
    public List<DayOfWeek> DaysOfWeek { get; set; } = new();
}
