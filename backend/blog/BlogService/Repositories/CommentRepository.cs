using BlogService.Data;
using BlogService.Models;
using BlogService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogService.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly BlogDbContext _db;

    public CommentRepository(BlogDbContext db)
    {
        _db = db;
    }

    public async Task<Comment> CreateAsync(Comment comment)
    {
        await _db.Comments.AddAsync(comment);
        await _db.SaveChangesAsync();
        return comment;
    }

    public async Task<Comment?> GetByIdAsync(Guid id)
    {
        return await _db.Comments
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Comment>> GetByBlogIdAsync(Guid blogId)
    {
        return await _db.Comments
            .Where(c => c.BlogId == blogId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task UpdateAsync(Comment comment)
    {
        _db.Comments.Update(comment);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        Comment? comment = await _db.Comments
            .FirstOrDefaultAsync(c => c.Id == id);

        if (comment == null)
            return;

        _db.Comments.Remove(comment);
        await _db.SaveChangesAsync();
    }

}
