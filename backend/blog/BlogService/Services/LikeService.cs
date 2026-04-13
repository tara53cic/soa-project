using BlogService.DTOs;
using BlogService.Models;
using BlogService.Repositories.Interfaces;
using BlogService.Services.Interfaces;

namespace BlogService.Services;

public class LikeService : ILikeService
{
    private readonly IBlogRepository _blogRepository;
    private readonly ILikeRepository _likeRepository;

    public LikeService(IBlogRepository blogRepository, ILikeRepository likeRepository)
    {
        _blogRepository = blogRepository;
        _likeRepository = likeRepository;
    }

    public async Task ToggleLikeAsync(Guid blogId, string username)
    {
        var existingLike = await _likeRepository
            .IsLikedByUserAsync(blogId, username);

        if (existingLike != null)
        {
            await _likeRepository.RemoveLikeAsync(blogId, username);
        }
        else
        {
            var like = new Like
            {
                Id = Guid.NewGuid(),
                BlogId = blogId,
                Username = username
            };

            await _likeRepository.AddLikeAsync(like);
        }
    }
}
