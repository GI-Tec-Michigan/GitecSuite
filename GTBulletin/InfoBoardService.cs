using Gitec.Models.GTBulletin;
using GTBulletin.Data;

namespace GTBulletin;

public class InfoBoardService
{
    
    private readonly ILogger<InfoBoardService> _logger;
    private InfoBoardDbContext _context;

    public InfoBoardService(ILogger<InfoBoardService> logger, InfoBoardDbContext context)
    {
        _logger = logger;
        _context = context;
        _logger.LogInformation("InfoBoard Service initialized");
    }
    
    public List<InfoBoard> Boards => _context.GetPublishedInfoBoards();
    
    public List<InfoBoardItem> GetItems(Guid boardUid)
    {
        return _context.GetPublishedInfoBoardItems(boardUid);
    }
    
    
}