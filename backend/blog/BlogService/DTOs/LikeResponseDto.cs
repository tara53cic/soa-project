namespace BlogService.DTOs;

public class LikeResponseDto
{
    public Guid BlogId { get; set; }
    public int LikesCount { get; set; }
    public bool IsLikedByCurrentUser { get; set; }
}
