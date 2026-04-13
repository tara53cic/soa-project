using BlogService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BlogService.DTOs;

namespace BlogService.Controllers;

[ApiController]
[Route("api")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet("blogs/{blogId:guid}/comments")]
    public async Task<IActionResult> GetByBlogId(Guid blogId)
    {
        var comments = await _commentService.GetByBlogIdAsync(blogId);
        return Ok(comments);
    }

    [HttpPost("blogs/{blogId:guid}/comments")]
    public async Task<IActionResult> Create(Guid blogId, [FromBody] CreateCommentDto request)
    {
        var comment = await _commentService.CreateAsync(blogId, request);
        return Ok(comment);
    }

    [HttpPut("comments/{commentId:guid}")]
    public async Task<IActionResult> Update(Guid commentId, [FromBody] EditCommentDto request)
    {

        var updatedComment = await _commentService.UpdateAsync(commentId, request);
        return Ok(updatedComment);
    }
}
