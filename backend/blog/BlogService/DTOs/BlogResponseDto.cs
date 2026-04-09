namespace BlogService.DTOs;

public class BlogResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DescriptionHtml { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string AuthorUsername { get; set; } = string.Empty;
    public List<string> ImageUrls { get; set; } = new();
}