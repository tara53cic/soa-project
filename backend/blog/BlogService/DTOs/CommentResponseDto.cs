namespace BlogService.DTOs;

public class CommentResponseDto
{
    public Guid Id { get; set; }
    public Guid BlogId { get; set; }
    public string AuthorUsername { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? EditedAt { get; set; }
}
