using Gitec.Data;
using Gitec.Models;
using Gitec.Models.GTBulletin;
using Gitec.Services;
using Microsoft.EntityFrameworkCore;

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
        var entries = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            entry.Entity.UpdatedAt = DateTime.UtcNow;
        }
    }

    //
    public List<InfoBoard> GetPublishedInfoBoards()
    {
        return InfoBoards
            .Where(i => i.IsPublished)
            .OrderBy(i => i.SortOrder)
            .ToList();
    }

    public InfoBoard GetInfoBoard(Guid Uid) =>
        InfoBoards.FirstOrDefault(i => i.Uid == Uid) ?? throw new InvalidOperationException();

    public List<InfoBoardItem> GetPublishedInfoBoardItems(InfoBoard board)
    {
        return GetPublishedInfoBoardItems(board.Uid);
    }

    public List<InfoBoardItem> GetPublishedInfoBoardItems(Guid boardUid)
    {
        return GetInfoBoard(boardUid).InfoBoardItems
            .Where(i => i.IsPublished)
            .OrderBy(i => i.SortOrder)
            .ToList();
    }


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
                    SortOrder = i
                };
            
                for (int j = 0; j <= 3; j++)
                {
                    try
                    {
                        var item = new InfoBoardItemText($"Item - {i}x{j}]")
                        {
                            SortOrder = j
                        };
                        board.InfoBoardItems.Add(item);
                    } catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

                InfoBoards.Add(board);
            } catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        SaveChanges();
    }
}