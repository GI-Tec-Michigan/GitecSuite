using Gitec.Models.GTBulletin;
using GTBulletin.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GTBulletin.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly InfoBoardDbContext _dbContext;

    public IndexModel(ILogger<IndexModel> logger, InfoBoardDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public List<InfoBoard> PublishedInfoBoards { get; set; } = [];

    public void OnGet()
    {
        PublishedInfoBoards = _dbContext.GetPublishedInfoBoards().ToList();
    }
}