namespace BlogService.Models;

public class Comment
{
    public Guid Id { get; set; }
    public Guid BlogId { get; set; }
    public string Text { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime EditedAt { get; set; }
}
