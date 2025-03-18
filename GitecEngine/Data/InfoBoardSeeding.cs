using GitecEngine.Enumerations;
using GitecEngine.InfoBoard.Models;
using GitecEngine.InfoBoard.Models.Boards;
using GitecEngine.InfoBoard.Models.Elements;
using Microsoft.EntityFrameworkCore;

namespace GitecEngine.Data;

public class InfoBoardSeeding
{
    private readonly InfoBoardDbContext _dbContext;

    public InfoBoardSeeding(InfoBoardDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();

    public async Task SeedDefaultDisplay(bool force = false)
    {
        if (_dbContext.Displays.Any() && !force) return;

        // Ensure we are in a transaction to avoid foreign key issues
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            // Deleting all existing data to start fresh
            await _dbContext.Database.ExecuteSqlRawAsync("DELETE FROM DisplayBoardRels");
            await _dbContext.Database.ExecuteSqlRawAsync("DELETE FROM BoardElementRels");
            await _dbContext.Database.ExecuteSqlRawAsync("DELETE FROM ScheduleDate");
            await _dbContext.Database.ExecuteSqlRawAsync("DELETE FROM ScheduleTime");
            await _dbContext.Database.ExecuteSqlRawAsync("DELETE FROM SchedulePackages");
            await _dbContext.Database.ExecuteSqlRawAsync("DELETE FROM Elements");
            await _dbContext.Database.ExecuteSqlRawAsync("DELETE FROM Boards");
            await _dbContext.Database.ExecuteSqlRawAsync("DELETE FROM Displays");

            // Create new display and board
            var display = new Display(title: "Main Display");
            var board1 = new ElementalBoard(title: "Main Board");
            var board2 = new ElementalBoard(title: "Second Board");

            // Elements
            var timeElement = new DateTimeElement(title: "Top Element")
            {
                DateTimeContent = DateTime.Now 
                
            };
            var markdownElement = new MarkdownElement(title: "Main Element")
            {
                MarkdownContent = "Test"
                
            };
            var calendarElement = new CalendarElement(title: "Calendar Element")
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                CalendarContent = "json text",
                View = CalendarView.Default
                
            };
            var htmlElement = new HtmlElement(title: "HTML Element")
            {
                HtmlContent = "<h1>HTML</h1>",
                Javascript = "",
                CssStyle = ""
                
            };
            var imageElement = new ImageElement(title: "Image Element")
            {
                ImageSrc = "https://via",
                ImageAlt = "Image",
                ImageCaption = "Image",
                TextPosition = Justify.Left
                
            };
            var videoElement = new VideoElement(title: "Video Element")
            {
                VideoSrc = "https://via",
                VideoCaption = "Video",
                VideoAlt = "Video",
                TextPosition = Justify.Right
            };
            var liveElement = new LiveDataElement(title: "Live Element")
            {
                LiveDataSrc = "https://via",
                LiveDataContent = "Live",
                LiveDataSubtitle = "Live"
            };

            // Schedule Packages
            var bSchedule = new SchedulePackage(title: "Weekdays All Day")
            {
                Dates = 
                [
                    new ScheduleDate(DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddYears(1)))
                ],
                Times =
                [
                    new ScheduleTime(TimeOnly.MinValue, TimeOnly.MaxValue)
                ],
                DaysOfWeek = 
                [
                    DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday
                ]
            };

            var dSchedule = new SchedulePackage(title: "Weekends All Day")
            {
                Dates =
                [
                    new ScheduleDate(DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddYears(1)))
                ],
                Times =
                [
                    new ScheduleTime(TimeOnly.MinValue, TimeOnly.MaxValue)
                ],
                DaysOfWeek = 
                [
                    DayOfWeek.Saturday, DayOfWeek.Sunday
                ]
            };

            board1.SchedulePackages = [bSchedule, dSchedule];
            board2.SchedulePackages = [bSchedule, dSchedule];

            // Establish relationships
            var displayBoardRels = new List<DisplayBoardRel>
            {
                new DisplayBoardRel { Display = display, Board = board1 },
                new DisplayBoardRel { Display = display, Board = board2 },
            };
            
            var boardElementRels = new List<BoardElementRel>
            {
                new() { Board = board1, Element = timeElement },
                new() { Board = board1, Element = markdownElement },
                new() { Board = board1, Element = calendarElement },
                new() { Board = board1, Element = htmlElement },
                new() { Board = board2, Element = imageElement },
                new() { Board = board2, Element = videoElement },
                new() { Board = board2, Element = liveElement }
            };
            

            // Add everything to DB
            _dbContext.AddRange(bSchedule, dSchedule);
            _dbContext.AddRange(display, board1, board2);
            _dbContext.AddRange(timeElement, markdownElement, calendarElement, htmlElement, imageElement, videoElement, liveElement);
            _dbContext.AddRange(displayBoardRels);
            _dbContext.AddRange(boardElementRels);
            
            await SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception("Seeding failed", ex);
        }
    }
}
