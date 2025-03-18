using GitecEngine.Enumerations;
using GitecEngine.InfoBoard.Models;
using GitecEngine.InfoBoard.Models.Boards;
using GitecEngine.InfoBoard.Models.Elements;
using GitecEngine.Models;
using GitecEngine.Services;
using Microsoft.EntityFrameworkCore;

namespace GitecEngine.Data;

/// <summary>
/// 
/// builder.Services.AddDbContext<InfoBoardDbContext>(options =>
/// options.UseLazyLoadingProxies().UseSqlite("Data Source=InfoBoard.db"));
///
/// </summary>

public class InfoBoardDbContext : DbContext
{
    private readonly ConfigurationService _configService;

    public DbSet<Board> Boards { get; set; }
    public DbSet<Display> Displays { get; set; }
    public DbSet<Element> Elements { get; set; }
    public DbSet<BoardElementRel> BoardElementRels { get; set; }
    public DbSet<DisplayBoardRel> DisplayBoardRels { get; set; }
    public DbSet<SchedulePackage> SchedulePackages { get; set; }

    public InfoBoardDbContext(DbContextOptions<InfoBoardDbContext> options, ConfigurationService configService)
        : base(options)
    {
        _configService = configService;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;
        var connectionString = _configService.GetConnectionString();
        optionsBuilder.UseSqlite(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Define Many-to-Many Relationship: Display ⇔ Board (via DisplayBoardRel)
        modelBuilder.Entity<DisplayBoardRel>()
            .HasOne(dbr => dbr.Display)
            .WithMany(d => d.DisplayBoardRelations)
            .HasForeignKey(dbr => dbr.DisplayOid);

        modelBuilder.Entity<DisplayBoardRel>()
            .HasOne(dbr => dbr.Board)
            .WithMany(b => b.DisplayBoardRelations)
            .HasForeignKey(dbr => dbr.BoardOid);

        // Define Many-to-Many Relationship: Board ⇔ Element (via BoardElementRel)
        modelBuilder.Entity<BoardElementRel>()
            .HasOne(ber => ber.Board)
            .WithMany(b => b.BoardElementRelations)
            .HasForeignKey(ber => ber.BoardOid);

        modelBuilder.Entity<BoardElementRel>()
            .HasOne(ber => ber.Element)
            .WithMany(e => e.BoardElementRelations)
            .HasForeignKey(ber => ber.ElementOid);

        // Define One-to-Many Relationship: Board ⇔ SchedulePackage
        modelBuilder.Entity<Board>()
            .HasMany(b => b.SchedulePackages)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        // Define One-to-Many Relationship: Element ⇔ SchedulePackage
        modelBuilder.Entity<Element>()
            .HasMany(e => e.SchedulePackages)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        // Inheritance Mapping for Board (Discriminator Column: "board_type")
        modelBuilder.Entity<Board>()
            .HasDiscriminator<string>("board_type")
            .HasValue<AlertBoard>(BoardType.Alert.ToString())
            .HasValue<AnnounceBoard>(BoardType.Announcement.ToString())
            .HasValue<ElementalBoard>(BoardType.Elemental.ToString())
            .HasValue<MediaBoard>(BoardType.Media.ToString());

        // Inheritance Mapping for Element (Discriminator Column: "element_type")
        modelBuilder.Entity<Element>()
            .HasDiscriminator<string>("element_type")
            .HasValue<MarkdownElement>(ElementType.Markdown.ToString())
            .HasValue<HtmlElement>(ElementType.Html.ToString())
            .HasValue<ImageElement>(ElementType.Image.ToString())
            .HasValue<VideoElement>(ElementType.Video.ToString())
            .HasValue<LiveDataElement>(ElementType.LiveData.ToString())
            .HasValue<CalendarElement>(ElementType.Calendar.ToString());

        // Configure SchedulePackage to Own Lists of ScheduleDate and ScheduleTime
        modelBuilder.Entity<SchedulePackage>()
            .OwnsMany(sp => sp.Dates, d =>
            {
                d.Property(p => p.StartDate).HasColumnType("DATE");
                d.Property(p => p.EndDate).HasColumnType("DATE");
            });

        modelBuilder.Entity<SchedulePackage>()
            .OwnsMany(sp => sp.Times, t =>
            {
                t.Property(p => p.StartTime).HasColumnType("TEXT"); // SQLite does not support TimeOnly natively
                t.Property(p => p.EndTime).HasColumnType("TEXT");
            });
    }


    /// <summary>
    /// Override SaveChanges to update UpdatedAt timestamp
    /// </summary>
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
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            entry.Entity.UpdatedAt = DateTime.UtcNow;
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow; // Ensure CreatedAt is set for new entities
            }
        }
    }
    
    
}