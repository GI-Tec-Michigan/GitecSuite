using Gitec.Models;
using Gitec.Models.GTBulletin;
using Gitec.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GTBulletin.Data;

public class InfoBoardDbContext : IdentityDbContext
{
    public InfoBoardDbContext(DbContextOptions<InfoBoardDbContext> options) : base(options) { }
    
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
            .HasValue<InfoBoardItem>(InfoBoardItemType.Text) 
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
            optionsBuilder.UseSqlite($"Data Source={ConfigurationService.GetDatabaseFile()}"); // Ensure SQLite is configured
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

    public InfoBoard GetInfoBoard(Guid Uid) => InfoBoards.FirstOrDefault(i => i.Uid == Uid) ?? throw new InvalidOperationException();

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
}