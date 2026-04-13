using BlogService.DTOs;

namespace BlogService.Services.Interfaces;

public interface ILikeService
{
    Task<LikeResponseDto> AddLikeAsync(Guid blogId, Guid currentUserId);
    Task<LikeResponseDto> RemoveLikeAsync(Guid blogId, Guid currentUserId);
}
