using System.ComponentModel.DataAnnotations;
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

// New Class for Date Ranges
public class ScheduleDate
{
    public ScheduleDate(DateOnly sDate, DateOnly eDate)
    {
        StartDate = sDate;
        EndDate = eDate;
    }

    public ScheduleDate() { } // Empty constructor for serialization
    
    [Key]
    public int Oid { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}

// New Class for Time Ranges
public class ScheduleTime
{
    public ScheduleTime(TimeOnly sTime, TimeOnly eTime)
    {
        StartTime = sTime;
        EndTime = eTime;
    }

    public ScheduleTime() { } // Empty constructor for serialization

    [Key]
    public int Oid { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}
