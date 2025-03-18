using GitecEngine.InfoBoard.Models;

namespace GitecEngine.InfoBoard.Services;

public interface IScheduleService
{
    SchedulePackage GetPackage(Guid uid);
    List<SchedulePackage> GetPackages(bool showInactive = false);
    SchedulePackage CreatePackage(string title);
    SchedulePackage CreatePackage(string title, List<DayOfWeek> days);
    SchedulePackage AddDateRange(ScheduleDate date, SchedulePackage schedulePackage);
    SchedulePackage RemoveDateRange(ScheduleDate date, SchedulePackage schedulePackage);
    SchedulePackage AddTimeRange(ScheduleTime time, SchedulePackage schedulePackage);
    SchedulePackage RemoveTimeRange(ScheduleTime time, SchedulePackage schedulePackage);
    SchedulePackage UpdateDaysOfWeek(List<DayOfWeek> days, SchedulePackage schedulePackage);
    SchedulePackage UpdatePackage(SchedulePackage schedulePackage);
    bool IsScheduled(Board board);
    bool IsScheduled(Element element);
}