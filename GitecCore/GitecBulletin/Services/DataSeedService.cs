using Gitec.ExceptionHandling;
using Gitec.GitecBulletin.Data;
using Gitec.GitecBulletin.Enums;
using Gitec.GitecBulletin.Models;
using Gitec.GitecBulletin.Models.UtilityModels;
using Microsoft.EntityFrameworkCore;

namespace Gitec.GitecBulletin.Services;

public class DataSeedService
{
    private readonly GitecBulletinDbContext _dbContext;
    
    private readonly ElementService _elementService;
    private readonly BoardService _boardService;
    private readonly DisplayThemeService _displayThemeService;
    private readonly SchedulePackageService _schedulePackageService;
    private readonly DisplayScreenService _displayScreenThemeService;

    public DataSeedService(GitecBulletinDbContext dbContext, ElementService elementService, BoardService boardService, DisplayThemeService displayThemeService, SchedulePackageService schedulePackageService, DisplayScreenService displayScreenThemeService)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _elementService = elementService ?? throw new ArgumentNullException(nameof(elementService));
        _boardService = boardService ?? throw new ArgumentNullException(nameof(boardService));
        _displayThemeService = displayThemeService ?? throw new ArgumentNullException(nameof(displayThemeService));
        _schedulePackageService = schedulePackageService ?? throw new ArgumentNullException(nameof(schedulePackageService));
        _displayScreenThemeService = displayScreenThemeService ?? throw new ArgumentNullException(nameof(displayScreenThemeService));
        _dbContext.Database.EnsureCreated(); // Ensure the database is created
    }

    public void SeedData(bool force = false)
    {
        SeedDisplayTheme(force);
        SeedDefaultSchedulePackage(force);
        SeedElement(force);
        SeedBoard(force);
        SeedDisplayScreen(force);
    }

    private T? EnsureDefaultEntity<T>(IQueryable<T> query, bool force) where T : class
    {
        var entity = query.FirstOrDefault();
        if (entity == null) return null;

        if (!force) return entity;
        _dbContext.Remove(entity);
        _dbContext.SaveChanges();
        return null;

    }

    private void SeedDisplayTheme(bool force = false)
    {
        if (EnsureDefaultEntity(_dbContext.DisplayThemes.Where(t => t.IsDefault), force) != null)
            return;
        
        _displayThemeService.CreateDisplayTheme(new DisplayTheme(CoreConstants.Default)
        {
            BgColor = System.Drawing.Color.LightGray,
            TextColor = System.Drawing.Color.DarkGray,
            AccentColor = System.Drawing.Color.Blue,
            FrameColor = System.Drawing.Color.Black,
            FrameAccentColor = System.Drawing.Color.White,
            ShowFrame = true,
            IsDefault = true
        });
    }

    private void SeedDefaultSchedulePackage(bool force = false)
    {
        if (EnsureDefaultEntity(_dbContext.SchedulePackages.Where(sp => sp.IsDefault), force) != null)
            return;
        
        _schedulePackageService.CreateSchedulePackage(new SchedulePackage(CoreConstants.Default)
        {
            ActiveDays = Enum.GetValues<DayOfWeek>().ToArray(),
            TimeWindows = [new TimeWindow { StartTime = TimeSpan.Zero, EndTime = TimeSpan.FromHours(24) }],
            IsDefault = true
        });
    }

    private void SeedElement(bool force = false)
    {
        if (EnsureDefaultEntity(_dbContext.Elements.Where(e => e.Title == CoreConstants.Default), force) != null)
            return;

        var defaultSchedule = _dbContext.SchedulePackages.FirstOrDefault(sp => sp.IsDefault)
            ?? throw new EntityNotFoundException("Default schedule package not found.");

        _elementService.CreateElement(new Element(CoreConstants.Default)
        {
            Type = ElementType.Markdown,
            Schedule = defaultSchedule
        });
    }

    private void SeedBoard(bool force = false)
    {
        if (EnsureDefaultEntity(_dbContext.Boards.Where(b => b.Title == CoreConstants.Default), force) != null)
            return;
     
        _boardService.CreateBoard(new Board(CoreConstants.Default)
        {
            DisplayTheme = _dbContext.DisplayThemes.FirstOrDefault(t => t.IsDefault),
            Schedule = _dbContext.SchedulePackages.FirstOrDefault(sp => sp.IsDefault),
            BoardType = BoardType.Elemental
        });
    }

    private void SeedDisplayScreen(bool force = false)
    {
        if (EnsureDefaultEntity(_dbContext.Displays.Where(d => d.Title == CoreConstants.Default), force) != null)
            return;

        var board = _dbContext.Boards.Include(b => b.Elements).FirstOrDefault(b => b.Title == CoreConstants.Default)
            ?? throw new EntityNotFoundException("Default board not found.");

        if (board.Elements.Count == 0)
            board.Elements.Add(_dbContext.Elements.FirstOrDefault(e => e.Title == CoreConstants.Default)
                ?? throw new EntityNotFoundException("Default element not found."));

        _displayScreenThemeService.CreateDisplay(new DisplayScreen(CoreConstants.Default)
        {
            Boards = new List<Board> { board },
            Location = "Main"
        });
    }
}
