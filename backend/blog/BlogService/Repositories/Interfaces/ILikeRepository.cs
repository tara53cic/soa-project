using BlogService.Models;

namespace BlogService.Repositories.Interfaces;

public interface ILikeRepository
{
    Task<Like> AddLikeAsync(Like like);
    Task<bool> IsLikedByUserAsync(Guid blogId, string username);
    Task<int> GetLikesCountByBlogIdAsync(Guid blogId);
    Task RemoveLikeAsync(Guid blogId, string username);
}
