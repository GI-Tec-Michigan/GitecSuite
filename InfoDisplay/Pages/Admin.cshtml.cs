using Gitec.Models.InfoDisplay;
using InfoDisplay.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InfoDisplay.Pages
{
    public class AdminModel : PageModel
    {
        private readonly ILogger<AdminModel> _logger;
        private readonly InfoDisplayDbContext _dbContext;
        public List<SelectListItem> InfoBoardOptions { get; set; } = [];
        public List<InfoBoardItem> InfoBoardItems { get; set; } = [];
        public InfoBoard SelectedInfoBoard { get; set; } = new InfoBoard("");

        public AdminModel(ILogger<AdminModel> logger, InfoDisplayDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            _logger.LogInformation("AdminModel instantiated");
        }

        public void OnGet()
        {
            var infoBoards = _dbContext.InfoBoards.Include(b => b.InfoBoardItems).ToList();
            InfoBoardOptions = infoBoards.Select(ib => new SelectListItem(ib.Title, ib.Uid.ToString())).ToList();

            if (infoBoards.Count == 0) return;
            InfoBoardItems = SelectedInfoBoard.InfoBoardItems.ToList();
        }

        public IActionResult OnGetGetInfoBoard(Guid uid)
        {
            var board = _dbContext.InfoBoards
                .Include(b => b.InfoBoardItems)
                .FirstOrDefault(b => b.Uid == uid);

            if (board == null)
            {
                return NotFound();
            }

            return new JsonResult(new
            {
                title = board.Title,
                name = board.Name,
                isPublished = board.IsPublished,
                infoBoardItems = board.InfoBoardItems.Select(i => new { i.Name, i.Uid }).ToList()
            });
        }

        public IActionResult OnPostSaveInfoBoard([FromBody] InfoBoard board)
        {
            if (board.Uid == Guid.Empty)
            {
                _dbContext.InfoBoards.Add(board);
            }
            else
            {
                _dbContext.InfoBoards.Update(board);
            }
            _dbContext.SaveChanges();
            return new JsonResult(new { uid = board.Uid });
        }

    }
}
