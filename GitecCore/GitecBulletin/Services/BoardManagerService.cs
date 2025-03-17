using Gitec.ExceptionHandling;
using Gitec.GitecBulletin.Data;
using Gitec.GitecBulletin.Enums;
using Gitec.GitecBulletin.Models;
using Microsoft.VisualStudio.Shell.Interop;

namespace Gitec.GitecBulletin.Services;

public class BoardManagerService
{
    private readonly GitecBulletinDbContext _dbContext;

    public BoardManagerService(GitecBulletinDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbContext.Database.EnsureCreated();
    }

    private void SaveChanges() => _dbContext.SaveChanges();

    // Displays ========================================================================================================

    #region Display

    public IQueryable<DisplayScreen> GetDisplays(bool includeArchived = false) =>
        _dbContext.Displays.Where(d => includeArchived || !d.IsArchived);

    public DisplayScreen GetDisplay(string title) =>
        _dbContext.Displays.FirstOrDefault(d => d.Title == title) ??
        throw new EntityNotFoundException($"Display '{title}' not found.");

    public DisplayScreen GetDisplay(Guid id) =>
        _dbContext.Displays.FirstOrDefault(d => d.Uid == id) ??
        throw new EntityNotFoundException($"Display with ID '{id}' not found.");

    public DisplayScreen CreateDisplay(string title, string location)
    {
        _dbContext.Displays.Add(new DisplayScreen(title)
        {
            Location = location
        });
        SaveChanges();
        return GetDisplay(title);
    }

    public DisplayScreen UpdateDisplay(DisplayScreen display)
    {
        _dbContext.Displays.Update(display);
        SaveChanges();
        return GetDisplay(display.Uid);
    }

    public void ArchiveDisplay(DisplayScreen display)
    {
        display.IsArchived = true;
        _dbContext.Displays.Update(display);
        SaveChanges();
    }

    public void UnArchiveDisplay(DisplayScreen display)
    {
        display.IsArchived = false;
        _dbContext.Displays.Update(display);
        SaveChanges();
    }

    public void AddBoardToDisplay(DisplayScreen display, Board board)
    {
        var order = display.Boards.Max(x => x.SortOrder);
        if (order < 0) order = 0; // Ensure order starts from 0 if no boards exist
        board.SortOrder = order;
        display.Boards.Add(board);
        SaveChanges();
    }

    public void RemoveBoardFromDisplay(DisplayScreen display, Board board)
    {
        if(!display.Boards.Contains(board))
            throw new EntityNotFoundException($"Display with ID '{display.Uid}' does not exist.");
        display.Boards.Remove(board);
        SaveChanges();
    }
    #endregion

    // Boards ==========================================================================================================

    #region Boards

    public Board GetBoard(string title) =>
        _dbContext.Boards.FirstOrDefault(b => b.Title == title) ??
        throw new EntityNotFoundException($"Board '{title}' not found.");

    public Board GetBoard(Guid id) =>
        _dbContext.Boards.FirstOrDefault(b => b.Uid == id) ??
        throw new EntityNotFoundException($"Board with ID '{id}' not found.");

    public Board CreateBoard(string title)
    {
        if (_dbContext.Boards.Any(b => b.Title == title))
            throw new InvalidOperationException($"Board with title '{title}' already exists.");
        
        _dbContext.Boards.Add(new Board(title)
        {
            DisplayTheme = _dbContext.DisplayThemes.FirstOrDefault(t => t.IsDefault) 
                ?? throw new EntityNotFoundException("Default theme not found."),
            Schedule = _dbContext.SchedulePackages.FirstOrDefault(sp => sp.IsDefault) 
                ?? throw new EntityNotFoundException("Default schedule package not found."),
            BoardType = BoardType.Elemental
        });
        SaveChanges();
        return GetBoard(title);
    }

    public Board UpdateBoard(Board board)
    {
        if (_dbContext.Boards.Any(b => b.Title == board.Title && b.Uid != board.Uid))
            throw new InvalidOperationException($"Board with title '{board.Title}' already exists.");
        board.DisplayTheme ??= _dbContext.DisplayThemes.FirstOrDefault(t => t.IsDefault);
        board.Schedule ??= _dbContext.SchedulePackages.FirstOrDefault(sp => sp.IsDefault);
        _dbContext.Boards.Update(board);
        SaveChanges();
        return GetBoard(board.Uid);
    }

    public void ArchiveBoard(Board board)
    {
        board.IsArchived = true;
        _dbContext.Boards.Update(board);
        SaveChanges();
    }

    public void UnArchiveBoard(Board board)
    {
        board.IsArchived = false;
        _dbContext.Boards.Update(board);
        SaveChanges();
    }

    public void AddElementToBoard(Board board, Element element)
    {
        board.Elements.Add(element);
        SaveChanges();
    }

    public void RemoveElementFromBoard(Board board, Element element)
    {
        if (!board.Elements.Remove(element))
            throw new InvalidOperationException($"Element '{element.Title}' not found in board '{board.Title}'.");
        SaveChanges();
    }

    #endregion

    // Elements ========================================================================================================

    #region Element

    public IQueryable<Element> GetElements(bool includeArchived = false) =>
        _dbContext.Elements.Where(e => includeArchived || !e.IsArchived);

    public Element GetElement(string title) =>
        _dbContext.Elements.FirstOrDefault(e => e.Title == title) ??
        throw new EntityNotFoundException($"Element '{title}' not found.");

    public Element GetElement(Guid id) =>
        _dbContext.Elements.FirstOrDefault(e => e.Uid == id) ??
        throw new EntityNotFoundException($"Element with ID '{id}' not found.");

    public Element CreateElement(string title)
    {
        _dbContext.Elements.Add(new Element(title));
        SaveChanges();
        return GetElement(title);
    }

    public Element UpdateElement(Element element)
    {
        _dbContext.Elements.Update(element);
        SaveChanges();
        return GetElement(element.Uid);
    }

    public void ArchiveElement(Element element)
    {
        element.IsArchived = true;
        _dbContext.Elements.Update(element);
        SaveChanges();
    }

    public void UnArchiveElement(Element element)
    {
        element.IsArchived = false;
        _dbContext.Elements.Update(element);
        SaveChanges();
    }

    #endregion


    // Themes ==========================================================================================================

    #region DisplayTheme

    public IQueryable<DisplayTheme> GetDisplayThemes(bool includeArchived = false) =>
        _dbContext.DisplayThemes.Where(theme => includeArchived || !theme.IsArchived);

    public DisplayTheme GetDisplayTheme(string name) =>
        _dbContext.DisplayThemes.FirstOrDefault(theme => theme.Title == name) 
        ?? throw new InvalidOperationException();
    
    public DisplayTheme GetDisplayTheme(Guid id) =>
        _dbContext.DisplayThemes.FirstOrDefault(theme => theme.Uid == id) 
        ?? throw new InvalidOperationException();
    
    public DisplayTheme CreateDisplayTheme(string title)
    {
        _dbContext.DisplayThemes.Add(new DisplayTheme(title));
        SaveChanges();
        return GetDisplayTheme(title);
    }
    
    public DisplayTheme UpdateDisplayTheme(DisplayTheme displayTheme)
    {
        _dbContext.DisplayThemes.Update(displayTheme);
        SaveChanges();
        return GetDisplayTheme(displayTheme.Uid);
    }
    
    public DisplayTheme GetDefaultDisplayTheme() =>
        _dbContext.DisplayThemes.FirstOrDefault(theme => theme.IsDefault) 
        ?? throw new EntityNotFoundException("Default theme not found.");
    
    public void ArchiveDisplayTheme(DisplayTheme displayTheme)
    {
        if(IsDisplayThemeInUse(displayTheme.Uid))
            throw new InvalidOperationException($"Cannot archive theme '{displayTheme.Title}' as it is currently in use.");
        
        displayTheme.IsArchived = true;
        _dbContext.DisplayThemes.Update(displayTheme);
        SaveChanges();
    }
    
    public void UnArchiveDisplayTheme(DisplayTheme displayTheme)
    {
        displayTheme.IsArchived = false;
        _dbContext.DisplayThemes.Update(displayTheme);
        SaveChanges();
    }

    private bool IsDisplayThemeInUse(Guid uid) =>
        _dbContext.Boards.Any(b => b.DisplayTheme.Uid == uid);
    
   
    #endregion
    
    // Schedule ========================================================================================================
    
    #region Schedule
    
    public IQueryable<SchedulePackage> GetSchedulePackages() =>
        _dbContext.SchedulePackages;
    
    public SchedulePackage GetSchedulePackage(string name) =>
        _dbContext.SchedulePackages.FirstOrDefault(sp => sp.Title == name) 
        ?? throw new EntityNotFoundException($"Schedule package '{name}' not found.");
    
    public SchedulePackage GetSchedulePackage(Guid id) =>
        _dbContext.SchedulePackages.FirstOrDefault(sp => sp.Uid == id) 
        ?? throw new EntityNotFoundException($"Schedule package with ID '{id}' not found.");
    
    public void CreateSchedulePackage(string title) =>
        _dbContext.SchedulePackages.Add(new SchedulePackage(title));
    
    public void UpdateSchedulePackage(SchedulePackage schedulePackage) =>
        _dbContext.SchedulePackages.Update(schedulePackage);
    
    public SchedulePackage GetDefaultSchedulePackage() =>
        _dbContext.SchedulePackages.FirstOrDefault(sp => sp.IsDefault) 
        ?? throw new EntityNotFoundException("Default schedule package not found.");
    
    #endregion Schedule
}