using BlogService.DTOs;
using BlogService.Models;
using BlogService.Repositories.Interfaces;
using BlogService.Services.Interfaces;
using Markdig;
using System.Net.Http.Json;

namespace BlogService.Services;

public class BlogService : IBlogService
{
    private readonly IBlogRepository _blogRepo;
    private readonly HttpClient _httpClient;

    private static readonly MarkdownPipeline Pipeline =
        new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

    public BlogService(IBlogRepository blogRepo, HttpClient httpClient)
    {
        _blogRepo = blogRepo;
        _httpClient = httpClient;
    }

    public async Task<List<BlogResponseDto>> GetAllAsync(string? username = null)
    {
        var blogs = await _blogRepo.GetAllAsync();
        return blogs.Select(blog =>
        {
            var dto = ToResponse(blog);
            dto.IsLikedByCurrentUser = username != null && blog.Likes.Any(l => l.Username == username);
            return dto;
        }).ToList();
    }

    public async Task<BlogResponseDto?> GetByIdAsync(Guid id, string? username = null)
    {
        var blog = await _blogRepo.GetByIdAsync(id);
        if (blog == null) return null;

        var dto = ToResponse(blog);
        dto.IsLikedByCurrentUser = username != null && blog.Likes.Any(l => l.Username == username);
        return dto;
    }

    public async Task<BlogResponseDto> CreateAsync(CreateBlogDto req)
    {
        var blog = new Blog
        {
            Id = Guid.NewGuid(),
            Title = req.Title,
            Description = req.Description,
            CreatedAt = DateTime.UtcNow,
            AuthorUsername = req.AuthorUsername,
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
            AuthorUsername = blog.AuthorUsername,
            ImageUrls = blog.Images.Select(i => i.Url).ToList(),

            Comments = blog.Comments.Select(c => new CommentResponseDto
            {
                Id = c.Id,
                BlogId = c.BlogId,
                Text = c.Text,
                AuthorUsername = c.AuthorUsername,
                CreatedAt = c.CreatedAt
            }).ToList(),

            LikesCount = blog.Likes.Count
        };
    }

    public async Task<List<BlogResponseDto>> GetFeedAsync(string username)
    {
        var followingUsers = await _httpClient.GetFromJsonAsync<List<UserResponseDto>>(
            $"http://follow-service:8083/following?username={username}");

        var followingUsernames = followingUsers?
            .Select(u => u.Username)
            .ToList() ?? new List<string>();

        var blogs = await _blogRepo.GetAllAsync();

        return blogs
             .Where(b => followingUsernames.Contains(b.AuthorUsername) || b.AuthorUsername == username)
             .OrderByDescending(b => b.CreatedAt)
             .Select(blog =>
             {
                 var dto = ToResponse(blog);
                 dto.IsLikedByCurrentUser = blog.Likes.Any(l => l.Username == username);
                 return dto;
             })
             .ToList();
    }
}