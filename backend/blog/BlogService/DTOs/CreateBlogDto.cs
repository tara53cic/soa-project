namespace BlogService.DTOs;

public class CreateBlogDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public long AuthorId { get; set; }
    public List<string>? ImageUrls { get; set; }
}