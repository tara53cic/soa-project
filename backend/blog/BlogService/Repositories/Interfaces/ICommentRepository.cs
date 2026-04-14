using BlogService.Models;

namespace BlogService.Repositories.Interfaces;

public interface ICommentRepository
{
    Task<Comment> CreateAsync(Comment comment);
    Task<Comment?> GetByIdAsync(Guid id);
    Task<List<Comment>> GetByBlogIdAsync(Guid blogId);
    Task UpdateAsync(Comment comment);
    Task DeleteAsync(Guid id);
}
