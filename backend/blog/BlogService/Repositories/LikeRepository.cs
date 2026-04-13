using BlogService.Data;
using BlogService.Models;
using BlogService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogService.Repositories;

public class LikeRepository : ILikeRepository
{
    private readonly BlogDbContext _db;

    public LikeRepository(BlogDbContext db)
    {
        _db = db;
    }

    public async Task<Like> CreateAsync(Like like)
    {
        await _db.Likes.AddAsync(like);
        await _db.SaveChangesAsync();
        return like;
    }

    public async Task<Like?> GetByBlogIdAndUsernameAsync(Guid blogId, string username)
    {
        return await _db.Likes
            .FirstOrDefaultAsync(l => l.BlogId == blogId && l.Username == username);
    }

    public async Task<int> GetLikesCountByBlogIdAsync(Guid blogId)
    {
        return await _db.Likes
            .CountAsync(l => l.BlogId == blogId);
    }

    public async Task DeleteAsync(Guid id)
    {
        Like? like = await _db.Likes
            .FirstOrDefaultAsync(l => l.Id == id);

        if (like == null)
            return;

        _db.Likes.Remove(like);
        await _db.SaveChangesAsync();
    }
}
