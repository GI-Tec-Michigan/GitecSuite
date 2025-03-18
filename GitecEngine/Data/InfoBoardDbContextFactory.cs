using GitecEngine.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GitecEngine.Data
{
    public class InfoBoardDbContextFactory : IDesignTimeDbContextFactory<InfoBoardDbContext>
    {
        public InfoBoardDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<InfoBoardDbContext>();
            optionsBuilder.UseSqlite("Data Source=InfoBoard.db"); // Hardcoded for design-time

            return new InfoBoardDbContext(optionsBuilder.Options, new ConfigurationService()); // Provide a dummy ConfigurationService if needed
        }
    }
}