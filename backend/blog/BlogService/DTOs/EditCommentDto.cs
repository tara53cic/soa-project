namespace BlogService.DTOs;

public class EditCommentDto
{
    public Guid CommentId { get; set; }
    public string Text { get; set; } = string.Empty;
}
