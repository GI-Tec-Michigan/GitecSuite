using Gitec.Models.GTBulletin;
using InfoDisplay.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InfoDisplay.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly InfoDisplayDbContext _dbContext;
    public List<InfoBoard> PublishedInfoBoards { get; set; } = [];
    public InfoBoard CurrentInfoBoard { get; set; }
    

    public IndexModel(ILogger<IndexModel> logger, InfoDisplayDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
        _dbContext.SeedData(true);
    }


    public void OnGet()
    {
        PublishedInfoBoards = _dbContext.GetPublishedInfoBoards().ToList();
        CurrentInfoBoard = PublishedInfoBoards.FirstOrDefault()!;
    }
}