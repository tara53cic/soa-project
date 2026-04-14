using BlogService.DTOs;

namespace BlogService.Services.Interfaces;

public interface ICommentService
{
    Task<CommentResponseDto> CreateAsync(Guid blogId, CreateCommentDto dto);
    Task<CommentResponseDto> UpdateAsync(Guid commentId, EditCommentDto dto);
    Task<List<CommentResponseDto>> GetByBlogIdAsync(Guid blogId);
}
