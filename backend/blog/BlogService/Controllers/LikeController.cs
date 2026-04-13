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
    public async Task<IActionResult> ToggleLike(Guid blogId)
    {
        string username = User.Identity.Name;

        await _likeService.ToggleLikeAsync(blogId, username);

        return Ok();
    }
}
