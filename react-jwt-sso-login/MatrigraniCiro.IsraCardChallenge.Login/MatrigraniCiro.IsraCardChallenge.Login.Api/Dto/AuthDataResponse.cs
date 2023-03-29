namespace MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Application.Dto;

public record AuthDataResponse
{
    public Guid LoginId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
