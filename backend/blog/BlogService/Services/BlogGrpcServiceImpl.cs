using BlogService.DTOs;
using BlogService.Protos;
using BlogService.Services.Interfaces;
using Grpc.Core;

namespace BlogService.Services;

public class BlogGrpcServiceImpl : BlogGrpcService.BlogGrpcServiceBase
{
    private readonly IBlogService _blogService;

    public BlogGrpcServiceImpl(IBlogService blogService)
    {
        _blogService = blogService;
    }

    public override async Task<BlogResponse> GetBlogById(GetBlogRequest request, ServerCallContext context)
    {
        var blog = await _blogService.GetByIdAsync(Guid.Parse(request.Id), request.Username);

        if (blog == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Blog post not found."));
        }

        return new BlogResponse
        {
            Id = blog.Id.ToString(),
            Title = blog.Title,
            Description = blog.Description,
            AuthorUsername = blog.AuthorUsername,
            CreatedAt = blog.CreatedAt.ToString("o"),
            LikesCount = blog.LikesCount
        };
    }

    public override async Task<BlogResponse> CreateBlog(CreateBlogRequest request, ServerCallContext context)
    {
        var dto = new CreateBlogDto
        {
            Title = request.Title,
            Description = request.Description,
            AuthorUsername = request.AuthorUsername,
            ImageUrls = request.ImageUrls.ToList()
        };

        var blog = await _blogService.CreateAsync(dto);

        return new BlogResponse
        {
            Id = blog.Id.ToString(),
            Title = blog.Title,
            Description = blog.Description,
            AuthorUsername = blog.AuthorUsername,
            CreatedAt = blog.CreatedAt.ToString("o")
        };
    }
}