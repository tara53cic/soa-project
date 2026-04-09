using BlogService.DTOs;

namespace BlogService.Services.Interfaces;
public interface IBlogService
{
    Task<List<BlogResponseDto>> GetAllAsync(); 
    Task<BlogResponseDto?> GetByIdAsync(Guid id); 
    Task<BlogResponseDto> CreateAsync(CreateBlogDto req);
}