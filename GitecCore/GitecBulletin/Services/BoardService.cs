using Gitec.ExceptionHandling;
using Gitec.GitecBulletin.Data;
using Gitec.GitecBulletin.Enums;
using Gitec.GitecBulletin.Models;

namespace Gitec.GitecBulletin.Services;

public class BoardService
{
    private readonly GitecBulletinDbContext _dbContext;
    private readonly DisplayThemeService _displayThemeService;
    private readonly SchedulePackageService _schedulePackageService;
    
    public BoardService(GitecBulletinDbContext dbContext, DisplayThemeService displayThemeService, SchedulePackageService schedulePackageService)
    {
        _dbContext = dbContext;
        _displayThemeService = displayThemeService;
        _schedulePackageService = schedulePackageService;
        _dbContext.Database.EnsureCreated();
    }
    private void SaveChanges() => _dbContext.SaveChanges();
    
    public Board GetBoard(string title) =>
        _dbContext.Boards.FirstOrDefault(b => b.Title == title) ?? throw new EntityNotFoundException("Board");

    public Board GetBoard(Guid id) =>
        _dbContext.Boards.FirstOrDefault(b => b.Uid == id) ?? throw new EntityNotFoundException("Board");

    public Board CreateBoard(Board board)
    {
        if (_dbContext.Boards.Any(b => b.Title == board.Title))
            throw new InvalidOperationException($"Board '{board.Title}' already exists.");

        board.DisplayTheme ??= _displayThemeService.GetDefaultDisplayTheme();
        board.Schedule ??= _schedulePackageService.GetDefaultSchedulePackage();
        
        _dbContext.Boards.Add(board);
        SaveChanges();
        return board;
    }
    
    public Board CreateBoard(string title)
    {
        return CreateBoard(new Board(title)
        {
            BoardType = BoardType.Elemental
        });
    }

    public Board UpdateBoard(Board board)
    {
        if (_dbContext.Boards.Any(b => b.Title == board.Title && b.Uid != board.Uid))
            throw new InvalidOperationException($"Board '{board.Title}' already exists.");

        board.DisplayTheme ??= _displayThemeService.GetDefaultDisplayTheme();
        board.Schedule ??= _schedulePackageService.GetDefaultSchedulePackage();
        
        _dbContext.Boards.Update(board);
        SaveChanges();
        return board;
    }

    public void ToggleBoardArchiveStatus(Board board, bool isArchived)
    {
        board.IsArchived = isArchived;
        _dbContext.Boards.Update(board);
        SaveChanges();
    }

    public void AddElementToBoard(Board board, Element element)
    {
        if (board.Elements.Any(e => e.Uid == element.Uid))
            throw new InvalidOperationException($"Element '{element.Title}' already exists in board '{board.Title}'.");

        element.SortOrder = board.Elements.Max(x => x.SortOrder) + 1;
        
        board.Elements.Add(element);
        SaveChanges();
    }

    public void RemoveElementFromBoard(Board board, Element element)
    {
        if (!board.Elements.Remove(element))
            throw new InvalidOperationException($"Element '{element.Title}' not found in board '{board.Title}'.");
        SaveChanges();
    }
}