using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gitec.ExceptionHandling;
using Gitec.GitecBulletin.Enums;
using Gitec.InfoDisplay.Models;
using Gitec.InfoDisplay.Services;

namespace InfoDisplay.Data
{
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
                .HasValue<InfoBoardItem>(InfoBoardItemType.File)
                .HasValue<InfoBoardItemText>(InfoBoardItemType.Text)
                .HasValue<InfoBoardItemHtml>(InfoBoardItemType.Html)
                .HasValue<InfoBoardItemImage>(InfoBoardItemType.Image)
                .HasValue<InfoBoardItemVideo>(InfoBoardItemType.Video)
                .HasValue<InfoBoardItemMarkdown>(InfoBoardItemType.Markdown)
                .HasValue<InfoBoardItemRssFeed>(InfoBoardItemType.RssFeed);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;

            try
            {
                string connectionString = ConfigurationService.GetConnectionString();
                Console.WriteLine($"Connecting to database: {connectionString}");
                optionsBuilder.UseSqlite(connectionString);
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Failed to configure database connection.", ex);
            }
        }

        #region Data Retrieval with Exception Handling

        public List<InfoBoard> GetInfoBoardsPublished()
        {
            try
            {
                return InfoBoards.Where(b => !b.IsDeleted && b.IsPublished)
                                 .OrderBy(b => b.SortOrder)
                                 .ToList();
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Failed to retrieve published InfoBoards.", ex);
            }
        }

        public List<InfoBoard> GetInfoBoardsAll()
        {
            try
            {
                return InfoBoards.Where(b => !b.IsDeleted)
                                 .OrderBy(b => b.SortOrder)
                                 .ToList();
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Failed to retrieve all InfoBoards.", ex);
            }
        }

        public InfoBoard GetInfoBoard(Guid Uid)
        {
            try
            {
                return InfoBoards.FirstOrDefault(i => i.Uid == Uid)
                    ?? throw new DatabaseOperationException($"InfoBoard with UID {Uid} not found.");
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Error fetching InfoBoard by UID.", ex);
            }
        }

        public InfoBoard GetInfoBoard(string name)
        {
            try
            {
                return InfoBoards.FirstOrDefault(i => i.Name == name)
                    ?? throw new DatabaseOperationException($"InfoBoard with name '{name}' not found.");
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Error fetching InfoBoard by name.", ex);
            }
        }

        #endregion
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


                    var textItem = new InfoBoardItemText($"Text BaseItem - {i}")
                    {
                        SortOrder = 2,
                        TextContent = "This is a text item"
                    };
                    board.InfoBoardItems.Add(textItem);

                    var imageItem = new InfoBoardItemImage($"Image BaseItem - {i}")
                    {
                        SortOrder = 3,
                        ImageUrl = "https://amplifyingperformance.com/wp-content/uploads/2023/07/img-balancing-school-sports-768x439.jpg",
                        ImageAlt = "5 Female Student Athletes",
                        ImageCaption = "How To Balance School and Sports as a High Performing Student Athlete",
                        ImageStory = "When a high achieving student is also a high performing athlete, they often wind up grappling with a unique set of challenges.\r\n\r\nThe worlds of academics and athletics can be demanding, intense, and fiercely competitive in their own right—and these student athletes are attempting to excel in both."
                    };
                    board.InfoBoardItems.Add(imageItem);

                    var markdownItem = new InfoBoardItemMarkdown($"Markdown BaseItem - {i}")
                    {
                        SortOrder = 4,
                        MarkdownContent = "# This is a markdown item\n\nThis is a paragraph"
                    };
                    board.InfoBoardItems.Add(markdownItem);

                    var rssFeedItem = new InfoBoardItemRssFeed($"RssFeed BaseItem - {i}")
                    {
                        SortOrder = 5,
                        RssFeedUrl = "https://www.nasa.gov/rss/dyn/breaking_news.rss"
                    };
                    board.InfoBoardItems.Add(rssFeedItem);

                    InfoBoards.Add(board);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                SaveChanges();
            }
        }

        #endregion

        #region Data Manipulation with Exception Handling

        public void SaveInfoBoard(InfoBoard board)
        {
            try
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
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Failed to save InfoBoard.", ex);
            }
        }

        #endregion

        #region Overrides with Error Handling

        public override int SaveChanges()
        {
            try
            {
                UpdateTimestamps();
                return base.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Error saving changes to the database.", ex);
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                UpdateTimestamps();
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Error saving changes asynchronously.", ex);
            }
        }

        private void UpdateTimestamps()
        {
            try
            {
                var entries = ChangeTracker.Entries<BaseItem>()
                    .Where(e => e.State == EntityState.Modified);

                foreach (var entry in entries)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Failed to update timestamps.", ex);
            }
        }

        #endregion
    }
}
