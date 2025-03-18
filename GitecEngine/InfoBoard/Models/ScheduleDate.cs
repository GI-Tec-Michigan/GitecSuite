using System.ComponentModel.DataAnnotations;

namespace GitecEngine.InfoBoard.Models;

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