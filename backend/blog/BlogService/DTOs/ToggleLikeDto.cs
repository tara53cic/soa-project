namespace BlogService.DTOs;

public class ToggleLikeDto
{
    public Guid BlogId { get; set; }
    public Guid UserId { get; set; }
}
