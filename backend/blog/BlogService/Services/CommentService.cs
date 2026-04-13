using BlogService.DTOs;
using BlogService.Models;
using BlogService.Repositories.Interfaces;
using BlogService.Services.Interfaces;
using System.Reflection.Metadata;

namespace BlogService.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IBlogRepository _blogRepository;

    public CommentService(ICommentRepository commentRepository, IBlogRepository blogRepository)
    {
        _commentRepository = commentRepository;
        _blogRepository = blogRepository;
    }

    public async Task<CommentResponseDto> CreateAsync(Guid blogId, CreateCommentDto dto)
    {
        Blog? blog = await _blogRepository.GetByIdAsync(blogId);
        if (blog == null)
            return null;

        Comment comment = new Comment
        {
            Id = Guid.NewGuid(),
            BlogId = blogId,
            AuthorUsername = dto.AuthorUsername,
            Text = dto.Text.Trim(),
            CreatedAt = DateTime.UtcNow,
            EditedAt = null
        };

        Comment newComment = await _commentRepository.CreateAsync(comment);
        return MapToDto(newComment);
    }

    public async Task<CommentResponseDto> UpdateAsync(Guid commentId, EditCommentDto dto)
    {
        Comment? existingComment = await _commentRepository.GetByIdAsync(commentId);
        if (existingComment == null)
            return null;

        existingComment.Text = dto.Text.Trim();
        existingComment.EditedAt = DateTime.UtcNow;

        await _commentRepository.UpdateAsync(existingComment);

        return MapToDto(existingComment);
    }

    public async Task<List<CommentResponseDto>> GetByBlogIdAsync(Guid blogId)
    {
        Blog? blog = await _blogRepository.GetByIdAsync(blogId);
        if (blog == null)
            return null;

        List<Comment> comments = await _commentRepository.GetByBlogIdAsync(blogId);

        return comments.Select(MapToDto).ToList();
    }

    private static CommentResponseDto MapToDto(Comment comment)
    {
        return new CommentResponseDto
        {
            Id = comment.Id,
            BlogId = comment.BlogId,
            AuthorUsername = comment.AuthorUsername,
            Text = comment.Text,
            CreatedAt = comment.CreatedAt,
            EditedAt = comment.EditedAt
        };
    }
}
