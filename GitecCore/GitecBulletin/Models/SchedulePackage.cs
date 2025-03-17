using Gitec.GitecBulletin.Data;
using Gitec.GitecBulletin.Models.UtilityModels;
using Gitec.Models;

namespace Gitec.GitecBulletin.Models;

public class SchedulePackage : EntityBase
{
    public SchedulePackage(string title) : base(title)
    {
    }
    public bool IsDefault { get; set; } = false;
    public ICollection<DayOfWeek> ActiveDays { get; set; } = [];
    public ICollection<TimeWindow> TimeWindows { get; set; } = [];
    public ICollection<DateRange> DateRanges { get; set; } = [];
}