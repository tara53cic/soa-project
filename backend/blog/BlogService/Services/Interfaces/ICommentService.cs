using BlogService.DTOs;

namespace BlogService.Services.Interfaces;

public interface ICommentService
{
    Task<CommentResponseDto> CreateAsync( Guid blogId, Guid currentUserId, CreateCommentDto dto);
    Task<CommentResponseDto> UpdateAsync(Guid commentId, Guid currentUserId, EditCommentDto dto);
    Task DeleteAsync(Guid commentId, Guid currentUserId);
    Task<List<CommentResponseDto>> GetByBlogIdAsync(Guid blogId);
}
