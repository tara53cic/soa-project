using BlogService.Data;
using BlogService.Models;
using BlogService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogService.Repositories;

public class BlogRepository : IBlogRepository
{
    private readonly BlogDbContext _db;

    public BlogRepository(BlogDbContext db)
    {
        _db = db;
    }

    public async Task<List<Blog>> GetAllAsync()
    {
        return await _db.Blogs
            .Include(b => b.Images)
            .Include(b => b.Comments)
            .ToListAsync();
    }

    public async Task<Blog?> GetByIdAsync(Guid id)
    {
        var blog = await _db.Blogs
            .Include(b => b.Images)
            .Include(b => b.Comments)
            .FirstOrDefaultAsync(b => b.Id == id);

        return blog;
    }

    public async Task<Blog> CreateAsync(Blog blog)
    {
        await _db.Blogs.AddAsync(blog);
        await _db.SaveChangesAsync();
        return blog;
    }

    public async Task UpdateAsync(Blog blog)
    {
        _db.Blogs.Update(blog);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var blog = await _db.Blogs.FindAsync(id);
        if (blog != null)
        {
            _db.Blogs.Remove(blog);
            await _db.SaveChangesAsync();
        }
    }
}