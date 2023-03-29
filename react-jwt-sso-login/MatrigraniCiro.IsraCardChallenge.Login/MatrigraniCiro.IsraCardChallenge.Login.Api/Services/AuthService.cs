using AutoMapper;
using MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Application.Dto;
using MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Repository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Application.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository authRepository;
    private readonly IMapper mapper;
    private readonly IConfiguration configuration;

    public AuthService(IMapper mapper, IAuthRepository repository, IConfiguration configuration)
    {
        this.authRepository = repository;
        this.mapper = mapper;
        this.configuration = configuration;
    }

    public async Task<bool> DeleteAuthData(Guid id)
    {
        return await authRepository.Delete(id);
    }

    public async Task<IEnumerable<AuthDataResponse>> GetAuthData()
    {
        return this.mapper.Map<IEnumerable<AuthDataResponse>>(await authRepository.GetAll());
    }

    public async Task<string> Login(LoginRequest login)
    {
        var issuer = configuration["Jwt:Issuer"];
        var audience = configuration["Jwt:Audience"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);

        var tokenHandler = new JwtSecurityTokenHandler();
        
        var loginResult = false;
        var authData = await authRepository.Get(login.Username, default);

        if (authData is null || !login.Password.Equals(authData.Password))
            throw new Exception("Failed Auth Exception");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, login.Username),
                new Claim(ClaimTypes.Name, login.Username),
                new Claim(JwtRegisteredClaimNames.Sub, login.Username),
                new Claim(JwtRegisteredClaimNames.Email, login.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(4),
            Audience = audience,
            Issuer = issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}

