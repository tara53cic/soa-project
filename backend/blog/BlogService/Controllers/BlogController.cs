using BlogService.DTOs;
using BlogService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogService.Controllers;

[ApiController]
[Route("api/blogs")]
public class BlogController : ControllerBase
{
    private readonly IBlogService _blogService;

    public BlogController(IBlogService blogService)
    {
        _blogService = blogService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? username = null)
    {
        var blogs = await _blogService.GetAllAsync(username);
        return Ok(blogs);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, [FromQuery] string? username = null)
    {
        var blog = await _blogService.GetByIdAsync(id, username);

        if (blog == null)
        {
            return NotFound(new { Message = "Blog not found" });
        }

        return Ok(blog);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBlogDto request)
    {
        var blog = await _blogService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = blog.Id }, blog);
    }
}