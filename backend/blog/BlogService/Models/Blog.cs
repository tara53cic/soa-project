using System.Xml.Linq;

namespace BlogService.Models;

public class Blog
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string AuthorUsername { get; set; } = string.Empty;

    public List<BlogImage> Images { get; set; } = new();

    public List<Comment> Comments { get; set; } = new();
    public List<Like> Likes { get; set; } = new();

}