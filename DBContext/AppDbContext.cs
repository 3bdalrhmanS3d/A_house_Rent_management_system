using Microsoft.EntityFrameworkCore;
using Tasken2.Models;
namespace Tasken2.DBContext
{
    public class AppDbContext : DbContext
    {

        // Add-Migration test9 -Context AppDbContext
        //  Update-Database -Context AppDbContext
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyRating> PropertyRatings { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<SearchHistory> SearchHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(p => p.CreatedAt)
                      .HasDefaultValueSql("getutcdate()");
            });

            modelBuilder.Entity<Property>(entity =>
            {
                entity.Property(p => p.CreatedAt)
                      .HasDefaultValueSql("getutcdate()");
                entity.HasOne(p => p.CreatedBy)
                      .WithMany(u => u.Properties)
                      .HasForeignKey(p => p.CreatedById)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Additional configurations can be added here
        }
    }
}
