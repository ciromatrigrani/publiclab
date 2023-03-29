using System.ComponentModel.DataAnnotations;

namespace MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Model;

public record AuthData
{
    [Key]
    public Guid LoginId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
