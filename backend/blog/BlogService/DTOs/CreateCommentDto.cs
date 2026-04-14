namespace BlogService.DTOs;

public class CreateCommentDto
{
    public string Text { get; set; } = string.Empty;
    public string AuthorUsername { get; set; } = string.Empty;
}
