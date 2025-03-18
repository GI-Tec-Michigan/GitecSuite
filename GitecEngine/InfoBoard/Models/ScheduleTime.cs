using System.ComponentModel.DataAnnotations;

namespace GitecEngine.InfoBoard.Models;

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