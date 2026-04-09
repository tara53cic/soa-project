namespace BlogService.Models;

public class BlogImage
{
    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public Guid BlogId { get; set; }
    public Blog Blog { get; set; } = null!;
}