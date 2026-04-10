using BlogService.Models;

namespace BlogService.Repositories.Interfaces;

public interface IBlogRepository
{
    Task<List<Blog>> GetAllAsync();
    Task<Blog?> GetByIdAsync(Guid id);
    Task<Blog> CreateAsync(Blog blog);
    Task UpdateAsync(Blog blog);
    Task DeleteAsync(Guid id);
}