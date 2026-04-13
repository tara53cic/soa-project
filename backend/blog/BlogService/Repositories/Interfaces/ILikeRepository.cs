using BlogService.Models;

namespace BlogService.Repositories.Interfaces;

public interface ILikeRepository
{
    Task<Like> CreateAsync(Like like);
    Task<Like?> GetByBlogIdAndUserIdAsync(Guid blogId, Guid userId);
    Task<int> GetLikesCountByBlogIdAsync(Guid blogId);
    Task DeleteAsync(Guid id);
}
