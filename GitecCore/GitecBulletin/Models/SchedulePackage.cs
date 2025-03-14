using Gitec.GitecBulletin.Data;
using Gitec.GitecBulletin.Models.UtilityModels;

namespace Gitec.GitecBulletin.Models;

public class SchedulePackage
{
    public DayOfWeek[] ActiveDays { get; set; } = [];
    public TimeWindow[] TimeWindows { get; set; } = [];
    public DateRange[] DateRanges { get; set; } = [];
    public DayOfYear[] ActiveDaysOfYear { get; set; } = [];
}