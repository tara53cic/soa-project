namespace BlogService.DTOs;

public class CreateCommentDto
{
    public Guid BlogId { get; set; }
    public string Text { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
}
