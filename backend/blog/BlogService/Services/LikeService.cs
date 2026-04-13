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

    public async Task<LikeResponseDto> AddLikeAsync(Guid blogId, string currentUsername)
    {
        Blog? blog = await _blogRepository.GetByIdAsync(blogId);
        if (blog == null)
            return null;

        Like? existingLike = await _likeRepository.GetByBlogIdAndUsernameAsync(blogId, currentUsername);
        if (existingLike != null)
            throw new Exception("You have already liked this blog.");

        Like like = new Like
        {
            Id = Guid.NewGuid(),
            BlogId = blogId,
            Username = currentUsername
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

    public async Task<LikeResponseDto> RemoveLikeAsync(Guid blogId, string currentUsername)
    {
        Blog? blog = await _blogRepository.GetByIdAsync(blogId);
        if (blog == null)
            return null;

        Like? existingLike = await _likeRepository.GetByBlogIdAndUsernameAsync(blogId, currentUsername);
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
