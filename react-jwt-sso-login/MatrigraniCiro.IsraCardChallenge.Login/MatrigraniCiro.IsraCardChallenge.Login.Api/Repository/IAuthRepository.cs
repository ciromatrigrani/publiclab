using MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Model;

namespace MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Repository;

public interface IAuthRepository
{
    Task<AuthData> Create(AuthData authData, CancellationToken token = default);
    Task<bool> Delete(Guid id, CancellationToken token = default);
    Task<AuthData> Get(string username, CancellationToken token = default);
    Task<IEnumerable<AuthData>> GetAll(CancellationToken token = default);
}