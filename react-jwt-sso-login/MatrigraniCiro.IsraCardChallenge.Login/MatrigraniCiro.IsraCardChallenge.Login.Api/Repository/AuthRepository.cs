using MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Model;
using Microsoft.EntityFrameworkCore;


namespace MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Repository;

public class AuthRepository : IAuthRepository
{
    private readonly PersonalInfoContext _context;

    public AuthRepository(PersonalInfoContext context)
    {
        this._context = context;
    }

    public async Task<IEnumerable<AuthData>> GetAll(CancellationToken token = default)
    {
        return await _context.AuthData.ToListAsync(token);
    }

    public async Task<AuthData> Get(string username, CancellationToken token = default)
    {
        return await _context.AuthData.Where(c => c.Username.Equals(username)).FirstAsync(token);
    }

    public async Task<AuthData> GetById(Guid id, CancellationToken token = default)
    {
        return await _context.AuthData.Where(c => c.LoginId.Equals(id)).FirstAsync(token);
    }

    public async Task<AuthData> Create(AuthData authData, CancellationToken token = default)
    {
        authData.LoginId = Guid.NewGuid();
        _context.AuthData.Add(authData);
        await ((DbContext)_context).SaveChangesAsync(token);
        return await GetById(authData.LoginId, token);
    }

    public async Task<bool> Delete(Guid id, CancellationToken token = default)
    {
        _context.AuthData.Remove(new AuthData { LoginId = id });
        return (await ((DbContext)_context).SaveChangesAsync(token) > 0);
    }
}
