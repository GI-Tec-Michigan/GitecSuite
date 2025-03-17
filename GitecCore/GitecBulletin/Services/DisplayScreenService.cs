using Gitec.ExceptionHandling;
using Gitec.GitecBulletin.Data;
using Gitec.GitecBulletin.Models;

namespace Gitec.GitecBulletin.Services;

public class DisplayScreenService
{
    private readonly GitecBulletinDbContext _dbContext;
    public DisplayScreenService(GitecBulletinDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    private void SaveChanges() => _dbContext.SaveChanges();
    
    private static T EnsureEntityExists<T>(IQueryable<T> query, string entityName, object key) where T : class =>
        query.FirstOrDefault() ?? throw new EntityNotFoundException($"{entityName} '{key}' not found.");
    
    public IQueryable<DisplayScreen> GetDisplays(bool includeArchived = false) =>
        _dbContext.Displays.Where(d => includeArchived || !d.IsArchived);

    public DisplayScreen GetDisplay(string title) =>
        EnsureEntityExists(_dbContext.Displays.Where(d => d.Title == title), "Display", title);

    public DisplayScreen GetDisplay(Guid id) =>
        EnsureEntityExists(_dbContext.Displays.Where(d => d.Uid == id), "Display", id);

    public DisplayScreen CreateDisplay(DisplayScreen display)
    {
        if (_dbContext.Displays.Any(d => d.Title == display.Title))
            throw new InvalidOperationException($"Display '{display.Title}' already exists.");

        _dbContext.Displays.Add(display);
        SaveChanges();
        return display;
    }
    
    public DisplayScreen CreateDisplay(string title, string location)
    {
        return CreateDisplay(new DisplayScreen(title) { Location = location });
    }

    public DisplayScreen UpdateDisplay(DisplayScreen display)
    {
        _dbContext.Displays.Update(display);
        SaveChanges();
        return display;
    }
    
    public void AddBoardToDisplay(DisplayScreen display, Board board)
    {
        if (display.Boards.Any(b => b.Uid == board.Uid))
            throw new InvalidOperationException($"Board '{board.Title}' already exists in display '{display.Title}'.");

        board.SortOrder = display.Boards.Max(x => x.SortOrder) + 1;
        
        display.Boards.Add(board);
        SaveChanges();
    }

    public void ToggleDisplayArchiveStatus(DisplayScreen display, bool isArchived)
    {
        display.IsArchived = isArchived;
        _dbContext.Displays.Update(display);
        SaveChanges();
    }
}