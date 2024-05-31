using Microsoft.EntityFrameworkCore;
using Tasken2.Models;
namespace Tasken2.DBContext
{
    public class AppDbContext : DbContext
    {

        // Add-Migration test8 -Context AppDbContext
        //  Update-Database -Context AppDbContext
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


        public DbSet<Person> persons { get; set; }
        public DbSet<Property> properties { get; set; }

        public DbSet<PropertyRating> PropertyRatings { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Area> Areas { get; set; }

        public DbSet<SearchHistory> searchHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
