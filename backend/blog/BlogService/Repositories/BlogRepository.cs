using BlogService.Data;
using BlogService.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogService.Repositories;

public class BlogRepository
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
            .ToListAsync();
    }

    public async Task<Blog?> GetByIdAsync(Guid id)
    {
        var blog = await _db.Blogs
            .Include(b => b.Images)
            .FirstOrDefaultAsync(b => b.Id == id);

        return blog;
    }
}