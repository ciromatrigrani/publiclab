namespace MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Application.Dto;

public record LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}
