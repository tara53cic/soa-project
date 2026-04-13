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

    public async Task<LikeResponseDto> AddLikeAsync(Guid blogId, Guid currentUserId)
    {
        Blog? blog = await _blogRepository.GetByIdAsync(blogId);
        if (blog == null)
            return null;

        Like? existingLike = await _likeRepository.GetByBlogIdAndUserIdAsync(blogId, currentUserId);
        if (existingLike != null)
            throw new Exception("You have already liked this blog.");

        Like like = new Like
        {
            Id = Guid.NewGuid(),
            BlogId = blogId,
            UserId = currentUserId
        };

        await _likeRepository.CreateAsync(like);

        int likesCount = await _likeRepository.GetLikesCountByBlogIdAsync(blogId);

        return new LikeResponseDto
        {
            BlogId = blogId,
            LikesCount = likesCount,
            IsLikedByCurrentUser = true
        };
    }

    public async Task<LikeResponseDto> RemoveLikeAsync(Guid blogId, Guid currentUserId)
    {
        Blog? blog = await _blogRepository.GetByIdAsync(blogId);
        if (blog == null)
            return null;

        Like? existingLike = await _likeRepository.GetByBlogIdAndUserIdAsync(blogId, currentUserId);
        if (existingLike == null)
            return null;

        await _likeRepository.DeleteAsync(existingLike.Id);

        int likesCount = await _likeRepository.GetLikesCountByBlogIdAsync(blogId);

        return new LikeResponseDto
        {
            BlogId = blogId,
            LikesCount = likesCount,
            IsLikedByCurrentUser = false
        };
    }
}
