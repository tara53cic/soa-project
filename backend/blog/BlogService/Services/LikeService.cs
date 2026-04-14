using BlogService.DTOs;
using BlogService.Models;
using BlogService.Repositories.Interfaces;
using BlogService.Services.Interfaces;

namespace BlogService.Services;

public class LikeService : ILikeService
{
    private readonly ILikeRepository _likeRepository;

    public LikeService(ILikeRepository likeRepository)
    {
        _likeRepository = likeRepository;
    }

    public async Task<bool> ToggleLikeAsync(Guid blogId, string username)
    {
        var existingLike = await _likeRepository
        .IsLikedByUserAsync(blogId, username);

        if (existingLike)
        {
            await _likeRepository.RemoveLikeAsync(blogId, username);
            return false; 
        }

        var like = new Like
        {
            Id = Guid.NewGuid(),
            BlogId = blogId,
            Username = username
        };

        await _likeRepository.AddLikeAsync(like);
        return true; 
    }
}
