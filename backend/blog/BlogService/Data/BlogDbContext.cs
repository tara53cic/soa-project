using BlogService.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogService.Data;

public class BlogDbContext : DbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }

    public DbSet<Blog> Blogs => Set<Blog>();
    public DbSet<BlogImage> BlogImages => Set<BlogImage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<BlogImage>()
            .HasOne<Blog>()
            .WithMany()
            .HasForeignKey(i => i.BlogId);
    }
}