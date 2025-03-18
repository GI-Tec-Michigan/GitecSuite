using GitecEngine.Enumerations;
using GitecEngine.Models;

namespace GitecEngine.InfoBoard.Models.Elements;

public class CalendarElement : Element
{
    public CalendarElement(string title) : base(title)
    {
        Type = ElementType.Calendar;
    }
    
    public string CalendarContent
    {
        get => Content;
        set => Content = value;
    }

    public CalendarView View { get; set; } = CalendarView.Default;
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    
}