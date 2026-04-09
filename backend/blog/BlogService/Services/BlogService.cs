using BlogService.DTOs;
using BlogService.Models;
using BlogService.Repositories;
using BlogService.Repositories.Interfaces;
using BlogService.Services.Interfaces;
using Markdig;

namespace BlogService.Services;

public class BlogService : IBlogService
{
    private readonly IBlogRepository _blogRepo;

    private static readonly MarkdownPipeline Pipeline =
        new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

    public BlogService(IBlogRepository blogRepo)
    {
        _blogRepo = blogRepo;
    }

    public async Task<List<BlogResponseDto>> GetAllAsync()
    {
        var blogs = await _blogRepo.GetAllAsync();
        return blogs.Select(ToResponse).ToList();
    }

    public async Task<BlogResponseDto?> GetByIdAsync(Guid id)
    {
        var blog = await _blogRepo.GetByIdAsync(id);
        if (blog == null) return null;

        return ToResponse(blog);
    }

    public async Task<BlogResponseDto> CreateAsync(CreateBlogDto req)
    {
        var blog = new Blog
        {
            Id = Guid.NewGuid(),
            Title = req.Title,
            Description = req.Description,
            CreatedAt = DateTime.UtcNow,
            AuthorId = req.AuthorId,
            Images = req.ImageUrls?.Select(url => new BlogImage
            {
                Id = Guid.NewGuid(),
                Url = url
            }).ToList() ?? new List<BlogImage>()
        };

        var created = await _blogRepo.CreateAsync(blog);
        return ToResponse(created);
    }

    private static BlogResponseDto ToResponse(Blog blog)
    {
        return new BlogResponseDto
        {
            Id = blog.Id,
            Title = blog.Title,
            Description = blog.Description,
            DescriptionHtml = Markdown.ToHtml(blog.Description, Pipeline),
            CreatedAt = blog.CreatedAt,
            AuthorId = blog.AuthorId,
            ImageUrls = blog.Images.Select(i => i.Url).ToList()
        };
    }
}