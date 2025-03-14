using Gitec.GitecBulletin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gitec.Models;

namespace Gitec.GitecBulletin.Data;

public class BulletinDbContext : DbContext
{
    public BulletinDbContext(DbContextOptions<BulletinDbContext> options) : base(options) { }

    public DbSet<DisplayScreen> Displays { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<Element> Elements { get; set; }
    public DbSet<ThemeDef> DisplayThemes { get; set; }

    public DbSet<XDisplayBoards> BulletinDisplayBoards { get; set; }  // Many-to-Many Display <-> Board
    public DbSet<XBoardElements> BulletinBoardElements { get; set; } // Many-to-Many Board <-> Element


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // EntityBase Configurations
        modelBuilder.Entity<EntityBase>()
            .HasKey(e => e.Uid);

        modelBuilder.Entity<EntityBase>()
            .Property(e => e.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<EntityBase>()
            .Property(e => e.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<EntityBase>()
            .Property(e => e.IsArchived)
            .HasDefaultValue(false);


        // 🔹 Configure Many-to-Many: BulletinDisplay <-> BulletinBoard
        modelBuilder.Entity<XDisplayBoards>()
            .HasKey(bdb => new { bdb.BulletinDisplayId, bdb.BulletinBoardId });

        modelBuilder.Entity<XDisplayBoards>()
            .HasOne(bdb => bdb.DisplayScreen)
            .WithMany(bd => bd.BulletinDisplayBoards)
            .HasForeignKey(bdb => bdb.BulletinDisplayId);

        modelBuilder.Entity<XDisplayBoards>()
            .HasOne(bdb => bdb.Board)
            .WithMany(bb => bb.BulletinDisplayBoards)
            .HasForeignKey(bdb => bdb.BulletinBoardId);


        // 🔹 Configure Many-to-Many: BulletinBoard <-> Element
        modelBuilder.Entity<XBoardElements>()
            .HasKey(bbe => new { bbe.BulletinBoardId, bbe.BulletinElementId });

        modelBuilder.Entity<Board>()
            .HasMany(bb => (IEnumerable<XBoardElements>)bb.BulletinBoardElements)
            .WithOne(bbe => bbe.Board)
            .HasForeignKey(bbe => bbe.BulletinBoardId);

        modelBuilder.Entity<XBoardElements>()
            .HasOne(bbe => bbe.Element)
            .WithMany()
            .HasForeignKey(bbe => bbe.BulletinElementId);

        // Define a single Color-to-String converter
        var colorConverter = new ValueConverter<Color, string>(
            v => v.Name,  // Convert Color to string
            v => Color.FromName(v) // Convert string to Color
        );

        // Apply Color conversion to all Color properties in ThemeDef
        var colorProperties = new[]
        {
            nameof(ThemeDef.BgColor),
            nameof(ThemeDef.TextColor),
            nameof(ThemeDef.AccentColor),
            nameof(ThemeDef.FrameColor),
            nameof(ThemeDef.FrameAccentColor)
        };

        var displayThemeEntity = modelBuilder.Entity<ThemeDef>();
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
