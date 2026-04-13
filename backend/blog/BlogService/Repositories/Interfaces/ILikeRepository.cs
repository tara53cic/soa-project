using BlogService.Models;

namespace BlogService.Repositories.Interfaces;

public interface ILikeRepository
{
    Task<Like> CreateAsync(Like like);
    Task<Like?> GetByBlogIdAndUsernameAsync(Guid blogId, string username);
    Task<int> GetLikesCountByBlogIdAsync(Guid blogId);
    Task DeleteAsync(Guid id);
}
