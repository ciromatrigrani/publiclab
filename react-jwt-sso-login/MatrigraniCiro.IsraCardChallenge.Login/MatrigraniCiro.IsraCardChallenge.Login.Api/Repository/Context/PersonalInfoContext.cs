using MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Model;
using Microsoft.EntityFrameworkCore;

namespace MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Repository;

// this is used for our verification tests, don't rename or change the access modifier
public class PersonalInfoContext : DbContext
{
    public PersonalInfoContext(DbContextOptions<PersonalInfoContext> options) : base(options)
    {
    }

    public DbSet<PersonalInfo> PersonalInfo { get; set; }

    public DbSet<AuthData> AuthData { get; set; }
}
