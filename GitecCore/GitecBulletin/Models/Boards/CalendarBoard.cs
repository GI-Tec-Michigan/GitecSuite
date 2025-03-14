using Gitec.GitecBulletin.Data;
using Gitec.GitecBulletin.Enums;

namespace Gitec.GitecBulletin.Models.Boards;

public class CalendarBoard : Board
{
    public CalendarBoard(string title) : base(title)
    {
        BoardType = Enums.BoardType.Calendar;
    }
    public CalendarView View { get; set; } = CalendarView.Default;
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
}

public enum CalendarView
{
    Default = 0, // week
    Month = 1,
    Week = 2,
    Day = 3,
    ThreeDay = 4
}