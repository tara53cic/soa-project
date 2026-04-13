using BlogService.DTOs;

namespace BlogService.Services.Interfaces;

public interface ILikeService
{
    Task<LikeResponseDto> AddLikeAsync(Guid blogId, string currentUsername);
    Task<LikeResponseDto> RemoveLikeAsync(Guid blogId, string currentUsername);
}
