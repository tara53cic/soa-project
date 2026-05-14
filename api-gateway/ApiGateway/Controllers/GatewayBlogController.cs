using ApiGateway.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers;

[ApiController]
[Route("api/blog-details")]
public class GatewayBlogController : ControllerBase
{
    private readonly BlogGrpcService.BlogGrpcServiceClient _blogGrpcClient;

    public GatewayBlogController(BlogGrpcService.BlogGrpcServiceClient blogGrpcClient)
    {
        _blogGrpcClient = blogGrpcClient;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, [FromQuery] string? username)
    {
        try
        {
            var request = new GetBlogRequest
            {
                Id = id,
                Username = username ?? ""
            };

            var response = await _blogGrpcClient.GetBlogByIdAsync(request);

            return Ok(new
            {
                id = response.Id,
                title = response.Title,
                description = response.Description,
                authorUsername = response.AuthorUsername,
                createdAt = response.CreatedAt,
                likesCount = response.LikesCount
            });
        }
        catch (RpcException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Status.Detail);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBlogRequest request)
    {
        try
        {
            var response = await _blogGrpcClient.CreateBlogAsync(request);
            return Ok(response);
        }
        catch (RpcException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Status.Detail);
        }
    }
}