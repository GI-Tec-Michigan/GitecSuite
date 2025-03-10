using Gitec.Data;
using Gitec.Models.InfoDisplay;
using Gitec.Services;
using Microsoft.EntityFrameworkCore;

// dotnet ef migrations add UpdateDbe --context InfoDisplayDbContext --output-dir Migrations/InfoDisplay
// dotnet ef database update --context InfoDisplayDbContext


namespace InfoDisplay.Data;

public class InfoDisplayDbContext : DbContext
{
    public InfoDisplayDbContext(DbContextOptions<InfoDisplayDbContext> options) : base(options)
    {
    }

    public InfoDisplayDbContext()
    {
    }

    public DbSet<InfoBoard> InfoBoards { get; set; }
    public DbSet<InfoBoardItem> InfoBoardItems { get; set; }
    public DbSet<InfoBoardItemText> InfoBoardItemTexts { get; set; }
    public DbSet<InfoBoardItemHtml> InfoBoardItemHtmls { get; set; }
    public DbSet<InfoBoardItemImage> InfoBoardItemImages { get; set; }
    public DbSet<InfoBoardItemVideo> InfoBoardItemVideos { get; set; }
    public DbSet<InfoBoardItemMarkdown> InfoBoardItemMarkdowns { get; set; }
    public DbSet<InfoBoardItemRssFeed> InfoBoardItemRssFeeds { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InfoBoard>()
            .HasMany(b => b.InfoBoardItems)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<InfoBoardItem>()
            .HasDiscriminator<InfoBoardItemType>(nameof(InfoBoardItem.Type))
            .HasValue<InfoBoardItem>(InfoBoardItemType.File) // Ensure unique discriminator
            .HasValue<InfoBoardItemText>(InfoBoardItemType.Text)
            .HasValue<InfoBoardItemHtml>(InfoBoardItemType.Html)
            .HasValue<InfoBoardItemImage>(InfoBoardItemType.Image)
            .HasValue<InfoBoardItemVideo>(InfoBoardItemType.Video)
            .HasValue<InfoBoardItemMarkdown>(InfoBoardItemType.Markdown)
            .HasValue<InfoBoardItemRssFeed>(InfoBoardItemType.RssFeed);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            Console.WriteLine(
                $"Connection string: {ConfigurationService.GetConnectionString()}"); // Ensure connection string is correct
            optionsBuilder.UseSqlite($"{ConfigurationService.GetConnectionString()}"); // Ensure SQLite is configured
        }
    }

    //

    private IQueryable<InfoBoard> getInfoBoardsQry()
    {
        return InfoBoards
            .OrderBy(b => b.SortOrder);
    }

    private IEnumerable<InfoBoard> getInfoBoards() =>
        getInfoBoardsQry().Where(x => x.IsDeleted == false);

    public List<InfoBoard> GetInfoBoardsPublished() =>
        getInfoBoards()
            .Where(b => b.IsPublished)
            .OrderBy(b => b.SortOrder)
            .ToList();

    public List<InfoBoard> GetInfoBoardsAll() =>
        getInfoBoards()
            .OrderBy(b => b.SortOrder)
            .ToList();

    public InfoBoard GetInfoBoard(InfoBoard board) =>
        GetInfoBoard(board.Uid);

    public InfoBoard GetInfoBoard(Guid Uid) =>
        InfoBoards.FirstOrDefault(i => i.Uid == Uid) ?? throw new InvalidOperationException();

    public InfoBoard GetInfoBoard(string name) =>
        InfoBoards.FirstOrDefault(i => i.Name == name) ?? throw new InvalidOperationException();


    //

    private IQueryable<InfoBoardItem> getInfoBoardItemsQry()
    {
        return InfoBoardItems
            .OrderBy(b => b.SortOrder);
    }

    private IEnumerable<InfoBoardItem> getInfoBoardItems() =>
        getInfoBoardItemsQry().Where(x => x.IsDeleted == false);

    public List<InfoBoardItem> GetInfoBoardItemsPublished() =>
        getInfoBoardItems()
            .Where(b => b.IsPublished)
            .OrderBy(b => b.SortOrder)
            .ToList();

    public List<InfoBoardItem> GetInfoBoardItemsAll() =>
        getInfoBoardItems()
            .OrderBy(b => b.SortOrder)
            .ToList();

    public InfoBoardItem GetInfoBoardItem(InfoBoardItem item) =>
        GetInfoBoardItem(item.Uid);

    public InfoBoardItem GetInfoBoardItem(Guid Uid) =>
        InfoBoardItems.FirstOrDefault(i => i.Uid == Uid) ?? throw new InvalidOperationException();

    public InfoBoardItem GetInfoBoardItem(string name) =>
        InfoBoardItems.FirstOrDefault(i => i.Name == name) ?? throw new InvalidOperationException();

    public List<InfoBoardItem> GetInfoBoardItemsByBoard(InfoBoard board) =>
        board.InfoBoardItems.ToList();

    public List<InfoBoardItem> GetInfoBoardItemsByBoard(Guid Uid) =>
        GetInfoBoard(Uid).InfoBoardItems.ToList();

    public List<InfoBoardItem> GetInfoBoardItemsByBoard(string name) =>
        GetInfoBoard(name).InfoBoardItems.ToList();

    //

    public void SaveInfoBoard(InfoBoard board)
    {
        if (board.Uid == Guid.Empty)
        {
            InfoBoards.Add(board);
        }
        else
        {
            InfoBoards.Update(board);
        }
        SaveChanges();
    }



    #region Seed Data

    public void SeedData(bool force = false)
    {
        if (force)
        {
            InfoBoards.RemoveRange(InfoBoards);
            SaveChanges();
        }
        
        if (InfoBoards.Any())
            return;
        
        
        for (int i = 0; i <= 3; i++)
        {
            try
            {
                var board = new InfoBoard($"Board - {i}")
                {
                    SortOrder = 1,
                    IsPublished = true
                };


                var textItem = new InfoBoardItemText($"Text Item - {i}")
                {
                    SortOrder = 2,
                    TextContent = "This is a text item"
                };
                board.InfoBoardItems.Add(textItem);

                var imageItem = new InfoBoardItemImage($"Image Item - {i}")
                {
                    SortOrder = 3,
                    ImageUrl = "https://amplifyingperformance.com/wp-content/uploads/2023/07/img-balancing-school-sports-768x439.jpg",
                    ImageAlt = "5 Female Student Athletes",
                    ImageCaption = "How To Balance School and Sports as a High Performing Student Athlete",
                    ImageStory = "When a high achieving student is also a high performing athlete, they often wind up grappling with a unique set of challenges.\r\n\r\nThe worlds of academics and athletics can be demanding, intense, and fiercely competitive in their own right—and these student athletes are attempting to excel in both."
                };
                board.InfoBoardItems.Add(imageItem);

                var markdownItem = new InfoBoardItemMarkdown($"Markdown Item - {i}")
                {
                    SortOrder = 4,
                    MarkdownContent = "# This is a markdown item\n\nThis is a paragraph"
                };
                board.InfoBoardItems.Add(markdownItem);

                var rssFeedItem = new InfoBoardItemRssFeed($"RssFeed Item - {i}")
                {
                    SortOrder = 5,
                    RssFeedUrl = "https://www.nasa.gov/rss/dyn/breaking_news.rss"
                };
                board.InfoBoardItems.Add(rssFeedItem);

                InfoBoards.Add(board);
            } catch (Exception e)
            {
                Console.WriteLine(e);
            }
        
            SaveChanges();
        }
    }

    #endregion

    #region Overrides

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries<BaseItem>()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            entry.Entity.UpdatedAt = DateTime.UtcNow;
        }
    }

    #endregion
}