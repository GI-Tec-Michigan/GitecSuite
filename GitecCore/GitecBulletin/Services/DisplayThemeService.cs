using Gitec.ExceptionHandling;
using Gitec.GitecBulletin.Data;
using Gitec.GitecBulletin.Models;

namespace Gitec.GitecBulletin.Services;

public class DisplayThemeService
{
    private readonly GitecBulletinDbContext _dbContext;
    public DisplayThemeService(GitecBulletinDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbContext.Database.EnsureCreated();
    }
    private void SaveChanges() => _dbContext.SaveChanges();
    
    public IQueryable<DisplayTheme> GetDisplayThemes(bool includeArchived = false) =>
        _dbContext.DisplayThemes.Where(theme => includeArchived || !theme.IsArchived);

    public DisplayTheme GetDisplayTheme(string name) =>
        _dbContext.DisplayThemes.FirstOrDefault(theme => theme.Name == name) ?? throw new EntityNotFoundException("DisplayTheme");

    public DisplayTheme GetDisplayTheme(Guid id) =>
        _dbContext.DisplayThemes.FirstOrDefault(x => x.Uid == id) ?? throw new EntityNotFoundException("DisplayTheme"); 

    public DisplayTheme CreateDisplayTheme(DisplayTheme displayTheme)
    {
        if (_dbContext.DisplayThemes.Any(theme => theme.Title == displayTheme.Title))
            throw new InvalidOperationException($"DisplayTheme '{displayTheme.Title}' already exists.");
        
        _dbContext.DisplayThemes.Add(displayTheme);
        SaveChanges();
        return displayTheme;
    }
    public DisplayTheme CreateDisplayTheme(string title)
    {
        return CreateDisplayTheme(new DisplayTheme(title));
    }

    public DisplayTheme UpdateDisplayTheme(DisplayTheme displayTheme)
    {
        _dbContext.DisplayThemes.Update(displayTheme);
        SaveChanges();
        return displayTheme;
    }

    public DisplayTheme GetDefaultDisplayTheme() =>
        _dbContext.DisplayThemes.FirstOrDefault(x => x.IsDefault) ?? throw new EntityNotFoundException("DefaultDisplayTheme");

    public void ToggleDisplayThemeArchiveStatus(DisplayTheme displayTheme, bool isArchived)
    {
        if (isArchived && IsDisplayThemeInUse(displayTheme.Uid))
            throw new InvalidOperationException($"Cannot archive theme '{displayTheme.Title}' as it is in use.");
        
        displayTheme.IsArchived = isArchived;
        _dbContext.DisplayThemes.Update(displayTheme);
        SaveChanges();
    }

    private bool IsDisplayThemeInUse(Guid uid) =>
        _dbContext.Boards.Any(b => b.DisplayTheme!.Uid == uid);
}