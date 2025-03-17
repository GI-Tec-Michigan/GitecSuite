using Gitec.ExceptionHandling;
using Gitec.GitecBulletin.Data;
using Gitec.GitecBulletin.Models;

namespace Gitec.GitecBulletin.Services;

public class SchedulePackageService
{
    private readonly GitecBulletinDbContext _dbContext;
    public SchedulePackageService(GitecBulletinDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    private void SaveChanges() => _dbContext.SaveChanges();
    
    private static T EnsureEntityExists<T>(IQueryable<T> query, string entityName, object key) where T : class =>
        query.FirstOrDefault() ?? throw new EntityNotFoundException($"{entityName} '{key}' not found.");
    
    public IQueryable<SchedulePackage> GetSchedulePackages() => _dbContext.SchedulePackages;

    public SchedulePackage GetSchedulePackage(string name) =>
        EnsureEntityExists(_dbContext.SchedulePackages.Where(sp => sp.Title == name), "SchedulePackage", name);

    public SchedulePackage GetSchedulePackage(Guid id) =>
        EnsureEntityExists(_dbContext.SchedulePackages.Where(sp => sp.Uid == id), "SchedulePackage", id);

    public SchedulePackage CreateSchedulePackage(SchedulePackage schedulePackage)
    {
        _dbContext.SchedulePackages.Add(schedulePackage);
        SaveChanges();
        return schedulePackage;
    }
    public SchedulePackage CreateSchedulePackage(string title)
    {
        return CreateSchedulePackage(new SchedulePackage(title));
    }

    public void UpdateSchedulePackage(SchedulePackage schedulePackage)
    {
        _dbContext.SchedulePackages.Update(schedulePackage);
        SaveChanges();
    }

    public SchedulePackage GetDefaultSchedulePackage() =>
        EnsureEntityExists(_dbContext.SchedulePackages.Where(sp => sp.IsDefault), "Default SchedulePackage", "Default");
}