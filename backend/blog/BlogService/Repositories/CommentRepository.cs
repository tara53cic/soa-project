using BlogService.Data;
using BlogService.Models;
using BlogService.Repositories.Interfaces;
using MongoDB.Driver;

namespace BlogService.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly MongoDbContext _context;

    public CommentRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<Comment> CreateAsync(Comment comment)
    {
        var update = Builders<Blog>.Update.Push(b => b.Comments, comment);
        await _context.Blogs.UpdateOneAsync(b => b.Id == comment.BlogId, update);
        return comment;
    }

    public async Task<Comment?> GetByIdAsync(Guid id)
    {
        var blog = await _context.Blogs
            .Find(b => b.Comments.Any(c => c.Id == id))
            .FirstOrDefaultAsync();

        return blog?.Comments.FirstOrDefault(c => c.Id == id);
    }

    public async Task<List<Comment>> GetByBlogIdAsync(Guid blogId)
    {
        var blog = await _context.Blogs.Find(b => b.Id == blogId).FirstOrDefaultAsync();
        return blog?.Comments.OrderByDescending(c => c.CreatedAt).ToList() ?? new List<Comment>();
    }

    public async Task UpdateAsync(Comment comment)
    {
        var filter = Builders<Blog>.Filter.And(
            Builders<Blog>.Filter.Eq(b => b.Id, comment.BlogId),
            Builders<Blog>.Filter.Eq("Comments.Id", comment.Id)
        );
        var update = Builders<Blog>.Update.Set("Comments.$", comment);

        await _context.Blogs.UpdateOneAsync(filter, update);
    }

    public async Task DeleteAsync(Guid id)
    {
        var update = Builders<Blog>.Update.PullFilter(b => b.Comments, c => c.Id == id);
        await _context.Blogs.UpdateOneAsync(b => b.Comments.Any(c => c.Id == id), update);
    }

}
