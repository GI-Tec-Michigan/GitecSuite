using Gitec.GitecBulletin.Data;
using Gitec.GitecBulletin.Enums;
using Gitec.GitecBulletin.Models;
using Gitec.GitecBulletin.Models.UtilityModels;

namespace Gitec.GitecBulletin.Services;

public class DataSeedService
{
    private readonly GitecBulletinDbContext _dbContext;
    
    private readonly string _defaultDisplayTitle = "Default";
    
    public DataSeedService(GitecBulletinDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbContext.Database.EnsureCreated(); // Ensure the database is created
    }
    
    public void SeedData(bool force = false)
    {
        // This method is intended to seed the database with initial data.
        // The implementation will depend on the specific requirements of your application.
        
        // Example: Add default schedule packages, board types, etc.
        
        // Note: Ensure that this method is idempotent, meaning it can be called multiple times without
        // causing duplicate entries or errors.
        SeedDisplayThemes(force);
        SeedDefaultSchedulePackage(force);
        SeedInitDisplay(force);
    }
    
    private void SeedDisplayThemes(bool force = false)
    {
        var theme = new DisplayTheme(_defaultDisplayTitle)
        {
            BgColor = System.Drawing.Color.LightGray,
            TextColor = System.Drawing.Color.DarkGray,
            AccentColor = System.Drawing.Color.Blue,
            FrameColor = System.Drawing.Color.Black,
            FrameAccentColor = System.Drawing.Color.White,
            ShowFrame = true,
            IsDefault = true
        };
        if (_dbContext.DisplayThemes.Any() && !force)
            return;
        if (_dbContext.DisplayThemes.Any(t => t.Title == theme.Title))
            return;
        _dbContext.DisplayThemes.Add(theme);
        _dbContext.SaveChanges();
    }
    
    private void SeedDefaultSchedulePackage(bool force = false)
    {
        if (_dbContext.SchedulePackages.Any() && !force)
            return;

        var schedulePackage = new SchedulePackage(_defaultDisplayTitle)
        {
            ActiveDays = [DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday],
            TimeWindows = [new TimeWindow
            {
                StartTime = TimeSpan.Zero,
                EndTime = TimeSpan.FromHours(24)
            }],
            IsDefault = true
        };

        _dbContext.SchedulePackages.Add(schedulePackage);
        _dbContext.SaveChanges();
    }

    private void SeedInitDisplay(bool force = false)
    {
        
        var element = new Element(_defaultDisplayTitle)
        {
            Content = "Welcome to the Gitec Bulletin Board!",
            Type = ElementType.Markdown,
            Schedule = _dbContext.SchedulePackages.FirstOrDefault(sp => sp.IsDefault) ?? throw new InvalidOperationException("Default schedule package not found.")
        };
        
        var board = new Board(_defaultDisplayTitle)
        {
            DisplayTheme = _dbContext.DisplayThemes.FirstOrDefault(t => t.IsDefault) ?? throw new InvalidOperationException("Default theme not found."),
            Schedule = _dbContext.SchedulePackages.FirstOrDefault(sp => sp.IsDefault) ?? throw new InvalidOperationException("Default schedule package not found."),
            BoardType = BoardType.Elemental,
            Elements = new List<Element> { element }
        };
        
        var display = new DisplayScreen(_defaultDisplayTitle)
        {
            Location = "Main"
        };

        if (_dbContext.Displays.Any() && !force)
            return;
        if (_dbContext.Boards.Any(b => b.Title == board.Title) && !force)
            return;
        if (_dbContext.Elements.Any(e => e.Title == element.Title) && !force)
            return;
        
        _dbContext.Boards.Add(board);
        _dbContext.Displays.Add(display);
        _dbContext.SaveChanges();
    }
    

    
}