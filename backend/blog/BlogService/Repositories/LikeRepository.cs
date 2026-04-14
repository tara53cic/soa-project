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

    public async Task<Like> AddLikeAsync(Like like)
    {
        await _db.Likes.AddAsync(like);
        await _db.SaveChangesAsync();
        return like;
    }

    public async Task<bool> IsLikedByUserAsync(Guid blogId, string username)
    {
        return await _db.Likes
            .AnyAsync(l => l.BlogId == blogId && l.Username == username);
    }

    public async Task<int> GetLikesCountByBlogIdAsync(Guid blogId)
    {
        return await _db.Likes
            .CountAsync(l => l.BlogId == blogId);
    }

    public async Task RemoveLikeAsync(Guid blogId, string username)
    {
        var like = await _db.Likes
            .FirstOrDefaultAsync(l => l.BlogId == blogId && l.Username == username);

        if (like != null)
        {
            _db.Likes.Remove(like);
            await _db.SaveChangesAsync();
        }
    }
}
