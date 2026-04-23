using ToursService.Models;
using Microsoft.EntityFrameworkCore;

namespace ToursService.Data
{
    public class ToursDbContext : DbContext
    {
        public ToursDbContext(DbContextOptions<ToursDbContext> options) : base(options) { }

        public DbSet<Tour> Tours { get; set; }
        public DbSet<KeyPoint> KeyPoints { get; set; }
        public DbSet<TourDuration> TourDurations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tour>().Property(t => t.Tags).HasColumnType("text[]");
        }
    }
}
