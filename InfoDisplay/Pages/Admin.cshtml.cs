using Gitec.Models.InfoDisplay;
using InfoDisplay.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InfoDisplay.Pages
{
    public class AdminModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly InfoDisplayDbContext _dbContext;
        public List<InfoBoard> PublishedInfoBoards { get; set; } = [];
        public InfoBoard CurrentInfoBoard { get; set; }


        public AdminModel(ILogger<IndexModel> logger, InfoDisplayDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            _logger.LogInformation("AdminModel instantiated");
        }

        public void OnGet()
        {
            PublishedInfoBoards = _dbContext.GetPublishedInfoBoards().ToList();
            CurrentInfoBoard = PublishedInfoBoards.FirstOrDefault()!;
        }
    }
}
