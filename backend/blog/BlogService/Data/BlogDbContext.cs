using BlogService.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogService.Data;

public class BlogDbContext : DbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }

    public DbSet<Blog> Blogs => Set<Blog>();
    public DbSet<BlogImage> BlogImages => Set<BlogImage>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Like> Likes => Set<Like>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BlogImage>(entity =>
        {
            entity.HasOne(i => i.Blog)
                .WithMany(b => b.Images)
                .HasForeignKey(i => i.BlogId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasOne(c => c.Blog)
                .WithMany(b => b.Comments)
                .HasForeignKey(c => c.BlogId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasOne(l => l.Blog)
                .WithMany(b => b.Likes)
                .HasForeignKey(l => l.BlogId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(l => new { l.BlogId, l.Username })
                  .IsUnique();
        });
    }
}