using Gitec.GitecBulletin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Drawing;
using Gitec.Models;

namespace Gitec.GitecBulletin.Data;

public class GitecBulletinDbContext : DbContext
{
    public GitecBulletinDbContext(DbContextOptions<GitecBulletinDbContext> options) : base(options) { }
    // Parameterless constructor for EF Core Migrations
    public GitecBulletinDbContext() { }
    public DbSet<DisplayScreen> Displays { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<Element> Elements { get; set; }
    public DbSet<DisplayTheme> DisplayThemes { get; set; }
    public DbSet<SchedulePackage> SchedulePackages { get; set; } // Not used yet, but here for future use


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // EntityBase Configurations
        modelBuilder.Entity<Board>()
            .HasOne(b => b.Schedule)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Board>()
            .HasOne(b => b.DisplayTheme)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Board>()
            .HasMany(b => b.Elements)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DisplayScreen>()
            .HasMany(ds => ds.Boards)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
        
        // Define a single Color-to-String converter
        var colorConverter = new ValueConverter<Color, string>(
            v => v.Name,  // Convert Color to string
            v => Color.FromName(v) // Convert string to Color
        );

        // Apply Color conversion to all Color properties in DisplayTheme
        var colorProperties = new[]
        {
            nameof(DisplayTheme.BgColor),
            nameof(DisplayTheme.TextColor),
            nameof(DisplayTheme.AccentColor),
            nameof(DisplayTheme.FrameColor),
            nameof(DisplayTheme.FrameAccentColor)
        };

        var displayThemeEntity = modelBuilder.Entity<DisplayTheme>();
        foreach (var prop in colorProperties)
        {
            displayThemeEntity
                .Property<Color>(prop)
                .HasConversion(colorConverter);
        }
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Configure the context to use a SQLite database for testing purposes
            optionsBuilder.UseSqlite("Data Source=bulletin.db");
        }
    }
}
