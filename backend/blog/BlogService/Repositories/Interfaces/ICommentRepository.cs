using BlogService.Models;

namespace BlogService.Repositories.Interfaces;

public interface ICommentRepository
{
    Comment Create(Comment comment);
    Comment? GetById(Guid id);
    List<Comment> GetByBlogId(Guid blogId);
    Comment Update(Comment comment);
    void Delete(Guid id);
}
