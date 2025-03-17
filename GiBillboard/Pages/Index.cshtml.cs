using Gitec.GitecBulletin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiBillboard.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly DisplayScreenService _displayScreenService;
    private readonly BoardService _boardService;
    private readonly ElementService _elementService;
    private readonly SchedulePackageService _schedulePackageService;
    private readonly DisplayThemeService _displayThemeService;
    

    public IndexModel(ILogger<IndexModel> logger, DisplayScreenService displayScreenService, BoardService boardService, ElementService elementService, SchedulePackageService schedulePackageService, DisplayThemeService displayThemeService)
    {
        _logger = logger;
        _displayScreenService = displayScreenService;
        _boardService = boardService;
        _elementService = elementService;
        _schedulePackageService = schedulePackageService;
        _displayThemeService = displayThemeService;
        _logger.LogInformation("IndexModel constructor called.");
    }

    public void OnGet()
    {
        var display = _displayScreenService.GetDisplay("Main");
    }
}