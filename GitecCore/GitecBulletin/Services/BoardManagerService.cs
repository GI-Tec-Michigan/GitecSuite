using Gitec.ExceptionHandling;
using Gitec.GitecBulletin.Data;
using Gitec.GitecBulletin.Models;

namespace Gitec.GitecBulletin.Services;

public class BoardManagerService
{
    private readonly BulletinDbContext _dbContext;

    public BoardManagerService(BulletinDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbContext.Database.EnsureCreated();
    }

    private void SaveChanges() => _dbContext.SaveChanges();

    // Displays ==========================================================================================================

    #region Display

    public IQueryable<DisplayScreen> GetDisplays(bool includeArchived = false) =>
        _dbContext.Displays.Where(d => includeArchived || !d.IsArchived);

    public DisplayScreen GetDisplay(string title) =>
        _dbContext.Displays.FirstOrDefault(d => d.Title == title) ??
        throw new EntityNotFoundException($"Display '{title}' not found.");

    public DisplayScreen GetDisplay(Guid id) =>
        _dbContext.Displays.FirstOrDefault(d => d.Uid == id) ??
        throw new EntityNotFoundException($"Display with ID '{id}' not found.");

    public void CreateDisplay(string title, string location)
    {
        _dbContext.Displays.Add(new DisplayScreen(title, location)
        {
            Location = location
        });
        SaveChanges();
    }

    public void UpdateDisplay(DisplayScreen display)
    {
        _dbContext.Displays.Update(display);
        SaveChanges();
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
        var order = _dbContext.BulletinDisplayBoards
            .Where(bdb => bdb.BulletinDisplayId == display.Uid)
            .Select(bdb => (int?)bdb.Order)
            .Max() ?? 0;

        _dbContext.BulletinDisplayBoards.Add(new XDisplayBoards(display, board) { Order = order + 1 });
        SaveChanges();
    }

    public void RemoveBoardFromDisplay(DisplayScreen display, Board board)
    {
        var relation = _dbContext.BulletinDisplayBoards
            .FirstOrDefault(bdb => bdb.BulletinDisplayId == display.Uid && bdb.BulletinBoardId == board.Uid);

        if (relation != null)
        {
            _dbContext.BulletinDisplayBoards.Remove(relation);
            SaveChanges();
        }
    }
    #endregion

    // Boards ==========================================================================================================

    #region Boards

    public IQueryable<Board> GetBoardsByDisplay(DisplayScreen display) =>
        _dbContext.Boards.Where(b => !b.IsArchived &&
            b.BulletinDisplayBoards.Any(bdb => bdb.BulletinDisplayId == display.Uid));

    public IQueryable<Board> GetBoards(bool includeArchived = false) =>
        _dbContext.Boards.Where(b => includeArchived || !b.IsArchived);

    public Board GetBoard(string title) =>
        _dbContext.Boards.FirstOrDefault(b => b.Title == title) ??
        throw new EntityNotFoundException($"Board '{title}' not found.");

    public Board GetBoard(Guid id) =>
        _dbContext.Boards.FirstOrDefault(b => b.Uid == id) ??
        throw new EntityNotFoundException($"Board with ID '{id}' not found.");

    public void CreateBoard(string title)
    {
        _dbContext.Boards.Add(new Board(title));
        SaveChanges();
    }

    public void UpdateBoard(Board board)
    {
        _dbContext.Boards.Update(board);
        SaveChanges();
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
        _dbContext.BulletinBoardElements.Add(new XBoardElements(board, element));
        SaveChanges();
    }

    public void RemoveElementFromBoard(Board board, Element element)
    {
        var relation = _dbContext.BulletinBoardElements
            .FirstOrDefault(bbe => bbe.BulletinBoardId == board.Uid && bbe.BulletinElementId == element.Uid);
        if (relation != null)
        {
            _dbContext.BulletinBoardElements.Remove(relation);
            SaveChanges();
        }
    }

    #endregion

    // Elements ==========================================================================================================

    #region Element

    public IQueryable<Element> GetElementsByBoard(Board board) =>
        _dbContext.Elements.Where(e => !e.IsArchived &&
            _dbContext.BulletinBoardElements.Any(bbe => bbe.BulletinElementId == e.Uid && bbe.BulletinBoardId == board.Uid));

    public IQueryable<Element> GetElements(bool includeArchived = false) =>
        _dbContext.Elements.Where(e => includeArchived || !e.IsArchived);

    public Element GetElement(string title) =>
        _dbContext.Elements.FirstOrDefault(e => e.Title == title) ??
        throw new EntityNotFoundException($"Element '{title}' not found.");

    public Element GetElement(Guid id) =>
        _dbContext.Elements.FirstOrDefault(e => e.Uid == id) ??
        throw new EntityNotFoundException($"Element with ID '{id}' not found.");

    public void CreateElement(string title)
    {
        _dbContext.Elements.Add(new Element(title));
        SaveChanges();
    }

    public void UpdateElement(Element element)
    {
        _dbContext.Elements.Update(element);
        SaveChanges();
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

}