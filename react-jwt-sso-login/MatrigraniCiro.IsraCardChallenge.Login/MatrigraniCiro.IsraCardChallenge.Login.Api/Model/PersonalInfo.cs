using System.ComponentModel.DataAnnotations;

namespace MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Model;

public record PersonalInfo
{
    [Key]
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; }
}
