namespace BlogService.DTOs;

public class CreateBlogDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string AuthorUsername { get; set; } = string.Empty;
    public List<string>? ImageUrls { get; set; }
}