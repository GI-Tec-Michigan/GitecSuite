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
        _dbContext.Database.EnsureCreated();
    }
    private void SaveChanges() => _dbContext.SaveChanges();
    
    public IQueryable<SchedulePackage> GetSchedulePackages() => _dbContext.SchedulePackages;

    public SchedulePackage GetSchedulePackage(string name) =>
        _dbContext.SchedulePackages.SingleOrDefault(s => s.Name == name) ?? throw new EntityNotFoundException("SchedulePackage");

    public SchedulePackage GetSchedulePackage(Guid id) =>
        _dbContext.SchedulePackages.SingleOrDefault(s => s.Uid == id) ?? throw new EntityNotFoundException("SchedulePackage");

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
        _dbContext.SchedulePackages.SingleOrDefault(s => s.IsDefault) ?? throw new EntityNotFoundException("Default SchedulePackage");
}