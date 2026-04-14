using BlogService.DTOs;

namespace BlogService.Services.Interfaces;
public interface IBlogService
{
    Task<List<BlogResponseDto>> GetAllAsync(string? username); 
    Task<BlogResponseDto?> GetByIdAsync(Guid id, string? username); 
    Task<BlogResponseDto> CreateAsync(CreateBlogDto req);
}