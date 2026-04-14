using BlogService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
    public async Task<IActionResult> ToggleLike(Guid blogId, [FromQuery] string username)
    {  
        bool isLiked = await _likeService.ToggleLikeAsync(blogId, username);

        return Ok(new { liked = isLiked });
    }
}
