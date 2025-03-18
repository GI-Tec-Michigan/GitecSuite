using GitecEngine.Enumerations;
using GitecEngine.InfoBoard.Models;
using GitecEngine.InfoBoard.Models.Boards;
using GitecEngine.InfoBoard.Models.Elements;

namespace GitecEngine.InfoBoard.Services;

public interface IInfoBoardService
{
    // Display Management
    Display GetDisplay(Guid uid);
    List<Display> GetDisplays(bool showInactive = false);
    Display CreateDisplay(string title);
    Display UpdateDisplay(Display display);
    void ToggleDisplay(Guid uid);

    // Board Management
    Board GetBoard(Guid uid);
    List<Board> GetBoards(bool showInactive = false);
    List<Board> GetBoards(Display display, bool showInactive = false);
    Board CreateBoard(string title, BoardType type);
    Board UpdateBoard(Board board);
    void ToggleBoard(Guid uid);
    Board ToggleSchedulePackageToBoard(Board board, SchedulePackage schedulePackage);

    // Element Management
    Element GetElement(Guid uid);
    List<Element> GetElements(bool showInactive = false);
    List<Element> GetElements(Board board, bool showInactive = false);
    Element CreateElement(string title, string content, ElementType type);
    
    
    
    
    
    Element UpdateElement(Element element);
    void ToggleElement(Guid uid);
    Element ToggleSchedulePackageToElement(Element element, SchedulePackage schedulePackage);

    // Display Joins
    Display AddBoardToDisplay(Display display, Board board);
    Display AddBoardToDisplay(Guid displayUid, Guid boardUid);
    Display RemoveBoardFromDisplay(Display display, Board board);

    // Board Joins
    Board AddElementToBoard(Board board, Element element);
    Board AddElementToBoard(Guid boardUid, Guid elementUid);
    Board RemoveElementFromBoard(Board board, Element element);

    // Ordering
    void MoveBoardOrder(Display display, Board board, Direction dir);
    void MoveElementOrder(Board board, Element element, Direction dir);

   
    // Board Creation
    
    AlertBoard CreateAlertBoard(string title);
    AnnounceBoard CreateAnnounceBoard(string title);
    ElementalBoard CreateElementalBoard(string title);
    MediaBoard CreateMediaBoard(string title);
    
    // Element Creation
    
    MarkdownElement CreateMarkdownElement(string title);
    HtmlElement CreateHtmlElement(string title);
    ImageElement CreateImageElement(string title);
    LiveDataElement CreateLiveDataElement(string title);
    CalendarElement CreateCalendarElement(string title);
    VideoElement CreateVideoElement(string title);
    
}