using Gitec;
using Gitec.GitecBulletin.Data;
using Gitec.GitecBulletin.Models;
using Gitec.GitecBulletin.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace GiBillboard.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly DisplayScreenService _displayScreenService;
    private readonly BoardService _boardService;
    private readonly ElementService _elementService;
    private readonly SchedulePackageService _schedulePackageService;
    private readonly DisplayThemeService _displayThemeService;

    public DisplayScreen DisplayScreen { get; set; } // Initialize to avoid null issues

    public IndexModel(ILogger<IndexModel> logger, 
        DisplayScreenService displayScreenService, 
        BoardService boardService, 
        ElementService elementService, 
        SchedulePackageService schedulePackageService, 
        DisplayThemeService displayThemeService)
    {
        _logger = logger;
        _displayScreenService = displayScreenService;
        _boardService = boardService;
        _elementService = elementService;
        _schedulePackageService = schedulePackageService;
        _displayThemeService = displayThemeService;
    }

    public void OnGet()
    {
        _logger.LogInformation("Fetching DisplayScreen...");

        DisplayScreen = _displayScreenService.GetDisplay(CoreConstants.Default);
    }
}                   