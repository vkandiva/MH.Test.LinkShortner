using MH.Test.LinkShortner.WebAPIs.Models;
using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace MH.Test.LinkShortner.WebAPIs.DataLayer
{
    /// <summary>
    /// DB context class for the Database
    /// </summary>
    public class MortgageHouseDbContext : DbContext
    {
        public MortgageHouseDbContext(DbContextOptions<MortgageHouseDbContext> options) : base(options)
        {
        }

        public Microsoft.EntityFrameworkCore.DbSet<UrlMapping> UrlMappings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UrlMapping>()
                .HasKey(e => e.Id);
        }
    }
}
