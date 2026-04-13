namespace BlogService.Models;

public class Like
{
    public Guid Id { get; set; }
    public Guid BlogId { get; set; }
    public Blog Blog { get; set; } = null!;
    public string Username { get; set; } = string.Empty;
}
