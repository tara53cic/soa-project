using Grpc.Core;
using BlogService.Protos;
using BlogService.Services.Interfaces;

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
}