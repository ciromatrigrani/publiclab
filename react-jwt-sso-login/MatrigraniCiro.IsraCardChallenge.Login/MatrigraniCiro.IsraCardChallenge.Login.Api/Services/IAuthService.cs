using MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Application.Dto;

namespace MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Application.Services;

public interface IAuthService
{
    Task<string> Login(LoginRequest login);
    Task<bool> DeleteAuthData(Guid id);
    Task<IEnumerable<AuthDataResponse>> GetAuthData();
}