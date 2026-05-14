using ApiGateway.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers;

[ApiController]
[Route("gateway/blogs")]
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
            var request = new GetBlogRequest { Id = id, Username = username ?? "" };
            var response = await _blogGrpcClient.GetBlogByIdAsync(request);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
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
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}