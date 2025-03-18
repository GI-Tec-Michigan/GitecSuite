using GitecEngine.Data;
using GitecEngine.Enumerations;
using GitecEngine.ExceptionHandling;
using GitecEngine.InfoBoard.Models;
using GitecEngine.InfoBoard.Models.Boards;
using GitecEngine.InfoBoard.Models.Elements;

namespace GitecEngine.InfoBoard.Services;

public class InfoBoardService : IInfoBoardService
{
    private readonly InfoBoardDbContext _dbContext;

    public InfoBoardService(InfoBoardDbContext context)
    {
        _dbContext = context;
        //_dbContext.Database.EnsureCreated();
    }

    private void SaveChanges() => _dbContext.SaveChanges();


    // Display =========================================================================================================

    public Display GetDisplay(Guid uid) =>
        _dbContext.Displays.Find(uid) ?? throw new EntityNotFoundException(uid.ToString(), "Display");

    public List<Display> GetDisplays(bool showInactive = false) =>
        _dbContext.Displays.Where(d => showInactive || d.IsActive).ToList();


    public Display CreateDisplay(string title)
    {
        var display = new Display(title);
        _dbContext.Displays.Add(display);
        SaveChanges();
        return display;
    }

    public Display UpdateDisplay(Display display)
    {
        _dbContext.Displays.Update(display);
        SaveChanges();
        return display;
    }

    public void ToggleDisplay(Guid uid)
    {
        var display = GetDisplay(uid);
        display.IsActive = !display.IsActive;
        SaveChanges();
    }

    // Board ===========================================================================================================

    public Board GetBoard(Guid uid) =>
        _dbContext.Boards.Find(uid) ?? throw new EntityNotFoundException(uid.ToString(), "Board");
    
    public List<Board> GetBoards(bool showInactive = false) =>
        showInactive ? _dbContext.Boards.ToList() : _dbContext.Boards.Where(b => b.IsActive).ToList();
    
    public List<Board> GetBoards(Display display, bool showInactive = false) =>
        _dbContext.DisplayBoardRels
            .Where(d => d.DisplayOid == display.Uid && (showInactive || d.Board.IsActive))
            .Select(d => d.Board)
            .ToList();


    public Board CreateBoard(string title, BoardType type)
    {
        Board board = type switch
        {
            BoardType.Elemental => new ElementalBoard(title),
            BoardType.Announcement => new AnnounceBoard(title),
            BoardType.Alert => new AlertBoard(title),
            BoardType.Media => new MediaBoard(title),
            BoardType.Unknown => throw new UnknownTypeException(type.ToString()),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
        _dbContext.Boards.Add(board);
        SaveChanges();
        return board;
    }

    public Board UpdateBoard(Board board)
    {
        _dbContext.Boards.Update(board);
        SaveChanges();
        return board;
    }

    public void ToggleBoard(Guid uid)
    {
        var board = GetBoard(uid);
        board.IsActive = !board.IsActive;
        SaveChanges();
    }

    public Board ToggleSchedulePackageToBoard(Board board, SchedulePackage schedulePackage)
    {
        if (board.SchedulePackages!.Contains(schedulePackage))
            board.SchedulePackages.Remove(schedulePackage);
        else
            board.SchedulePackages.Add(schedulePackage);
        SaveChanges();
        return board;
    }

    // Element =========================================================================================================

    public Element GetElement(Guid uid) =>
        _dbContext.Elements.Find(uid) ?? throw new EntityNotFoundException(uid.ToString(), "Element");
    
    public List<Element> GetElements(bool showInactive = false) =>
        showInactive ? _dbContext.Elements.ToList() : _dbContext.Elements.Where(e => e.IsActive).ToList();
    
    public List<Element> GetElements(Board board, bool showInactive = false) =>
        showInactive ? _dbContext.BoardElementRels.Where(b => b.BoardOid == board.Uid)
            .Select(b => b.Element).ToList() : _dbContext.BoardElementRels.Where(b => b.BoardOid == board.Uid)
            .Select(b => b.Element).Where(e => e.IsActive).ToList();

    public Element CreateElement(string title, string content, ElementType type)
    {
        Element element = type switch
        {
            ElementType.Markdown => new MarkdownElement(title),
            ElementType.Html => new HtmlElement(title),
            ElementType.Image => new ImageElement(title),
            ElementType.Video => new VideoElement(title),
            ElementType.LiveData => new LiveDataElement(title),
            ElementType.Calendar => new CalendarElement(title),
            ElementType.Unknown => throw new UnknownTypeException(type.ToString()),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
        _dbContext.Elements.Add(element);
        SaveChanges();
        return element;
    }

    public Element UpdateElement(Element element)
    {
        _dbContext.Elements.Update(element);
        SaveChanges();
        return element;
    }

    public void ToggleElement(Guid uid)
    {
        var element = GetElement(uid);
        element.IsActive = !element.IsActive;
        SaveChanges();
    }

    public Element ToggleSchedulePackageToElement(Element element, SchedulePackage schedulePackage)
    {
        if(element.SchedulePackages!.Contains(schedulePackage))
            element.SchedulePackages.Remove(schedulePackage);
        else
        {
            element.SchedulePackages.Add(schedulePackage);
        }
        SaveChanges();
        return element;
    }

    // Display Joins ===========================================================================================================

    public Display AddBoardToDisplay(Display display, Board board)
    {
        _dbContext.DisplayBoardRels.Add(new DisplayBoardRel(display.Uid, board.Uid));
        SaveChanges();
        return display;
    }

    public Display AddBoardToDisplay(Guid displayUid, Guid boardUid)
    {
        var display = GetDisplay(displayUid);
        var board = GetBoard(boardUid);
        return AddBoardToDisplay(display, board);
    }

    public Display RemoveBoardFromDisplay(Display display, Board board)
    {
        var rel = _dbContext.DisplayBoardRels
            .FirstOrDefault(d => d.DisplayOid == display.Uid && d.BoardOid == board.Uid);
        if(rel == null) throw new EntityNotFoundException($"Board {board.Uid} does not exist");
        _dbContext.DisplayBoardRels.Remove(rel);
        SaveChanges();
        return display;
    }

    // Board Joins ===========================================================================================================

    public Board AddElementToBoard(Board board, Element element)
    {
        _dbContext.BoardElementRels.Add(new BoardElementRel(board.Uid, element.Uid));
        SaveChanges();
        return board;
    }

    public Board AddElementToBoard(Guid boardUid, Guid elementUid)
    {
        var board = GetBoard(boardUid);
        var element = GetElement(elementUid);
        return AddElementToBoard(board, element);
    }

    public Board RemoveElementFromBoard(Board board, Element element)
    {
        var rel = _dbContext.BoardElementRels
            .FirstOrDefault(b => b.BoardOid == board.Uid && b.ElementOid == element.Uid);
        if(rel == null) throw new EntityNotFoundException($"Element {element.Uid} does not exist");
        _dbContext.BoardElementRels.Remove(rel);
        SaveChanges();
        return board;
    }

    // Move ============================================================================================================

    public void MoveBoardOrder(Display display, Board board, Direction dir)
    {
        var rel = _dbContext.DisplayBoardRels
            .FirstOrDefault(d => d.DisplayOid == display.Uid && d.BoardOid == board.Uid);

        if (rel == null)
            throw new EntityNotFoundException(display.Uid.ToString(), "Display");

        var index = rel.OrderIndex;
        var newIndex = dir == Direction.Up ? index - 1 : index + 1;

        var maxIndex = _dbContext.DisplayBoardRels
            .Where(d => d.DisplayOid == display.Uid)
            .Select(d => d.OrderIndex) // Ensures EF Core translates this to SQL
            .DefaultIfEmpty(-1)
            .Max();

        if (newIndex < 0 || newIndex > maxIndex) return;

        var swap = _dbContext.DisplayBoardRels
            .Where(d => d.DisplayOid == display.Uid)
            .OrderBy(d => d.OrderIndex)
            .Skip(newIndex) // Uses Skip() instead of SkipWhile()
            .FirstOrDefault();

        if (swap == null) return;

        (rel.OrderIndex, swap.OrderIndex) = (swap.OrderIndex, rel.OrderIndex);
        SaveChanges();
    }


    public void MoveElementOrder(Board board, Element element, Direction dir)
    {
        var rel = _dbContext.BoardElementRels
            .FirstOrDefault(b => b.BoardOid == board.Uid && b.ElementOid == element.Uid);

        if (rel == null)
            throw new EntityNotFoundException($"Board {board.Uid} does not exist");

        var index = rel.OrderIndex;
        var newIndex = dir == Direction.Up ? index - 1 : index + 1;

        var maxIndex = _dbContext.BoardElementRels
            .Where(b => b.BoardOid == board.Uid)
            .Select(b => b.OrderIndex)
            .DefaultIfEmpty(-1)
            .Max();

        if (newIndex < 0 || newIndex > maxIndex) return;

        var swap = _dbContext.BoardElementRels
            .Where(b => b.BoardOid == board.Uid)
            .OrderBy(b => b.OrderIndex)
            .Skip(newIndex) // Uses Skip() instead of SkipWhile()
            .FirstOrDefault();

        if (swap == null) return;

        (rel.OrderIndex, swap.OrderIndex) = (swap.OrderIndex, rel.OrderIndex);
        SaveChanges();
    }

    


    
    
    // Create Boards ===============================================================================================
         
    public AlertBoard CreateAlertBoard(string title)
    {
        var board = new AlertBoard(title);
        _dbContext.Boards.Add(board);
        SaveChanges();
        return board;
    }
    
    public AnnounceBoard CreateAnnounceBoard(string title)
    {
        var board = new AnnounceBoard("Announcement Board");
        _dbContext.Boards.Add(board);
        SaveChanges();
        return board;
    }
    
    public ElementalBoard CreateElementalBoard(string title)
    {
        var board = new ElementalBoard("Elemental Board");
        _dbContext.Boards.Add(board);
        SaveChanges();
        return board;
    }
     
    public MediaBoard CreateMediaBoard(string title)
    {
        var board = new MediaBoard("Media Board")
        {
            MediaCaption = "Hello, World!",
            MediaContent = "Hello, World!",
            MediaStory = "https://example.com",
            MediaType = MediaType.Image
        };

        _dbContext.Boards.Add(board);
        SaveChanges();
        return board;
    }
    // Create Elements =============================================================================================
    
    public MarkdownElement CreateMarkdownElement(string title)
    {
        var element = new MarkdownElement(title)
        {
            MarkdownContent = "Hello, World!"
        };

        _dbContext.Elements.Add(element);
        SaveChanges();
        return element;
    }
    
    public HtmlElement CreateHtmlElement(string title)
    {
        var element = new HtmlElement(title)
        {
            HtmlContent = "Hello, World!",
            CssStyle = "display: block;",
            Javascript = "alert('Hello, World!');"
        };

        _dbContext.Elements.Add(element);
        SaveChanges();
        return element;
    }
    
    public ImageElement CreateImageElement(string title)
    {
        var element = new ImageElement(title)
        {
            ImageCaption = "Hello, World!",
            ImageSrc = "https://example.com/image.jpg",
            ImageAlt = "Hello, World!",
            TextPosition = Justify.Left
        };

        _dbContext.Elements.Add(element);
        SaveChanges();
        return element;
    }
    
    public VideoElement CreateVideoElement(string title)
    {
        var element = new VideoElement(title)
        {
            VideoCaption = "Hello, World!",
            VideoSrc = "https://example.com/video.mp4",
            TextPosition = Justify.Center
        };

        _dbContext.Elements.Add(element);
        SaveChanges();
        return element;
    }
    
    public LiveDataElement CreateLiveDataElement(string title)
    {
        var element = new LiveDataElement(title)
        {
            LiveDataSrc = "https://example.com/data.json",
            LiveDataSubtitle = "Hello, World!"
        };

        _dbContext.Elements.Add(element);
        SaveChanges();
        return element;
    }
    
    public CalendarElement CreateCalendarElement(string title)
    {
        var element = new CalendarElement(title)
        {
            CalendarContent = "Hello, World!",
            View = CalendarView.ThreeDay,
            StartDate = DateOnly.FromDateTime(DateTime.Now)
        };

        _dbContext.Elements.Add(element);
        SaveChanges();
        return element;
    }
    
    
}

