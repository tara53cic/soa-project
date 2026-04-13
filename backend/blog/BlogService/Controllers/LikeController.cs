using BlogService.DTOs;
using BlogService.Repositories.Interfaces;
using BlogService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace BlogService.Controllers;

[ApiController]
[Route("api/blogs/{blogId:guid}/likes")]

public class LikeController : ControllerBase
{
    private readonly ILikeService _likeService;

    public LikeController(ILikeService likeService)
    {
        _likeService = likeService;
    }

    [HttpPost]
    public async Task<IActionResult> AddLike(Guid blogId)
    {
        string currentUsername = GetCurrentUsername();
        var result = await _likeService.AddLikeAsync(blogId, currentUsername);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveLike(Guid blogId)
    {
        string currentUsername = GetCurrentUsername();
        var result = await _likeService.RemoveLikeAsync(blogId, currentUsername);
        return Ok(result);
    }

    private string GetCurrentUsername()
    {
        string? username = User.FindFirst(ClaimTypes.Name)?.Value
                           ?? User.FindFirst("sub")?.Value;

        if (string.IsNullOrWhiteSpace(username))
            throw new Exception("Username claim not found.");

        return username;
    }
}
