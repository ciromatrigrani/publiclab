using System.ComponentModel.DataAnnotations;

namespace MatrigraniCiro.DotNet6.MinimalApi.MinderaChallenge.BestBlogs.Model;

public record Post
{
    [Key]
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreationDate { get; set; }
}
