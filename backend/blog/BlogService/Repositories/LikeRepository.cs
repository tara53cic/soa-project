using BlogService.Data;
using BlogService.Models;
using BlogService.Repositories.Interfaces;
using MongoDB.Driver;

namespace BlogService.Repositories;

public class LikeRepository : ILikeRepository
{
    private readonly MongoDbContext _context;

    public LikeRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<Like> AddLikeAsync(Like like)
    {
        var update = Builders<Blog>.Update.Push(b => b.Likes, like);
        await _context.Blogs.UpdateOneAsync(b => b.Id == like.BlogId, update);
        return like;
    }

    public async Task<bool> IsLikedByUserAsync(Guid blogId, string username)
    {
        var blog = await _context.Blogs.Find(b => b.Id == blogId).FirstOrDefaultAsync();
        return blog?.Likes.Any(l => l.Username == username) ?? false;
    }

    public async Task<int> GetLikesCountByBlogIdAsync(Guid blogId)
    {
        var blog = await _context.Blogs.Find(b => b.Id == blogId).FirstOrDefaultAsync();
        return blog?.Likes.Count ?? 0;
    }

    public async Task RemoveLikeAsync(Guid blogId, string username)
    {
        var update = Builders<Blog>.Update.PullFilter(b => b.Likes, l => l.Username == username);
        await _context.Blogs.UpdateOneAsync(b => b.Id == blogId, update);
    }
}
