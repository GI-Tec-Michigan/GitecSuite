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
        _dbContext.Database.EnsureCreated();
    }
    private void SaveChanges() => _dbContext.SaveChanges();
    
    public IQueryable<DisplayScreen> GetDisplays(bool includeArchived = false) =>
        _dbContext.Displays.Where(d => includeArchived || !d.IsArchived);

    public DisplayScreen GetDisplay(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Display title cannot be null or empty.", nameof(title));
        
        var display = _dbContext.Displays.FirstOrDefault(d => d.Title == title);
        return display ?? throw new EntityNotFoundException("Display");
    }

    public DisplayScreen GetDisplay(Guid id) =>
        _dbContext.Displays.FirstOrDefault(x => x.Uid == id) ?? throw new EntityNotFoundException("Display");

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