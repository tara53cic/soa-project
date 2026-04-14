namespace BlogService.Models;

public class Comment
{
    public Guid Id { get; set; }
    public Guid BlogId { get; set; }
    public Blog Blog { get; set; } = null!;
    public string Text { get; set; } = string.Empty;
    public string AuthorUsername { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? EditedAt { get; set; }
}
