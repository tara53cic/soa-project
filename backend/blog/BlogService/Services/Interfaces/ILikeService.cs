using BlogService.DTOs;

namespace BlogService.Services.Interfaces;

public interface ILikeService
{
    Task ToggleLikeAsync(Guid blogId, string username);
}
