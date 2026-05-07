using BlogService.Data;
using BlogService.Models;
using BlogService.Repositories.Interfaces;
using MongoDB.Driver;

namespace BlogService.Repositories;

public class BlogRepository : IBlogRepository
{
    private readonly MongoDbContext _context;

    public BlogRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<List<Blog>> GetAllAsync()
    {
        return await _context.Blogs.Find(_ => true).ToListAsync();
    }

    public async Task<Blog?> GetByIdAsync(Guid id)
    {
        return await _context.Blogs.Find(b => b.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Blog> CreateAsync(Blog blog)
    {
        await _context.Blogs.InsertOneAsync(blog);
        return blog;
    }

    public async Task UpdateAsync(Blog blog)
    {
        await _context.Blogs.ReplaceOneAsync(b => b.Id == blog.Id, blog);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _context.Blogs.DeleteOneAsync(b => b.Id == id);
    }
}