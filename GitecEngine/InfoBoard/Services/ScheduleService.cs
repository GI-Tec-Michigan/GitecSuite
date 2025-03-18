using GitecEngine.Data;
using GitecEngine.ExceptionHandling;
using GitecEngine.InfoBoard.Models;

namespace GitecEngine.InfoBoard.Services;

public class ScheduleService : IScheduleService
{
    private readonly InfoBoardDbContext _dbContext;

    public ScheduleService(InfoBoardDbContext context)
    {
        _dbContext = context;
        //_dbContext.Database.EnsureCreated();
    }
    
    /// <summary>
    /// using (var scope = app.Services.CreateScope())
    /// {
    ///    var dbContext = scope.ServiceProvider.GetRequiredService<InfoBoardDbContext>();
    ///    dbContext.Database.Migrate(); // Ensures the schema is up to date
    /// }
    /// </summary>

    
    private void SaveChanges() => _dbContext.SaveChanges();
    
    public SchedulePackage GetPackage(Guid uid) =>
        _dbContext.SchedulePackages.Find(uid) ?? 
        throw new EntityNotFoundException(uid.ToString(), "SchedulePackage");
    
    public List<SchedulePackage> GetPackages(bool showInactive = false) =>
        _dbContext.SchedulePackages
            .Where(b => showInactive || b.IsActive)
            .ToList();

    public SchedulePackage CreatePackage(string title)
    {
        var schedulePackage = new SchedulePackage(title);
        _dbContext.SchedulePackages.Add(schedulePackage);
        SaveChanges();
        return schedulePackage;
    }

    public SchedulePackage CreatePackage(string title, List<DayOfWeek> days)
    {
        var schedulePackage = new SchedulePackage(title)
        {
            DaysOfWeek = days
        };
        _dbContext.SchedulePackages.Add(schedulePackage);
        SaveChanges();
        return schedulePackage;
    }
    
    public SchedulePackage AddDateRange(ScheduleDate date, SchedulePackage schedulePackage)
    {
        _dbContext.Attach(schedulePackage);
        schedulePackage.Dates.Add(date);
        SaveChanges();
        return schedulePackage;
    }
    
    public SchedulePackage RemoveDateRange(ScheduleDate date, SchedulePackage schedulePackage)
    {
        schedulePackage.Dates.Remove(date);
        UpdatePackage(schedulePackage);
        return schedulePackage;
    }
    
    public SchedulePackage AddTimeRange(ScheduleTime time, SchedulePackage schedulePackage)
    {
        schedulePackage.Times.Add(time);
        UpdatePackage(schedulePackage);
        return schedulePackage;
    }
    
    public SchedulePackage RemoveTimeRange(ScheduleTime time, SchedulePackage schedulePackage)
    {
        schedulePackage.Times.Remove(time);
        UpdatePackage(schedulePackage);
        return schedulePackage;
    }
    
    public SchedulePackage UpdateDaysOfWeek(List<DayOfWeek> days, SchedulePackage schedulePackage)
    {
        schedulePackage.DaysOfWeek = days;
        UpdatePackage(schedulePackage);
        return schedulePackage;
    }
    
    public SchedulePackage UpdatePackage(SchedulePackage schedulePackage)
    {
        _dbContext.SchedulePackages.Update(schedulePackage);
        SaveChanges();
        return schedulePackage;
    }
    
    public bool IsScheduled(Board board)
    {
        if(board.SchedulePackages!.Count == 0)
            return false;
        return board.IsActive && IsScheduled(board.SchedulePackages);
    }
    
    public bool IsScheduled(Element element)
    {
        if(element.SchedulePackages!.Count == 0)
            return false;
        return element.IsActive && IsScheduled(element.SchedulePackages);
    }
    
    private static bool IsScheduled(List<SchedulePackage> packages)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var nowTime = TimeOnly.FromDateTime(DateTime.UtcNow);
        var todayDay = today.DayOfWeek;

        foreach (var pkg in packages)
        {
            var isDateValid = pkg.Dates.Count == 0 || pkg.Dates.Any(dt => dt.StartDate <= today && dt.EndDate >= today);
            var isTimeValid = pkg.Times.Count == 0 || pkg.Times.Any(tm => tm.StartTime <= nowTime && tm.EndTime >= nowTime);
            var isDayValid = pkg.DaysOfWeek.Count == 0 || pkg.DaysOfWeek.Contains(todayDay);
            if (isDateValid && isTimeValid && isDayValid)
                return true;
        }

        return false;
    }
}