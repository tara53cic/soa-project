using BlogService.DTOs;

namespace BlogService.Services.Interfaces;

public interface ILikeService
{
    Task<bool> ToggleLikeAsync(Guid blogId, string username);
}
