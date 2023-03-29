
using MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Model;

namespace MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Repository.Context;

public static class ContextInitialiser 
{

    public static void Run(PersonalInfoContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        context.AuthData.Add(new AuthData() { LoginId = Guid.NewGuid(), Username = "admin", Password = "teste" });
        context.AuthData.Add(new AuthData() { LoginId = Guid.NewGuid(), Username = "admin1", Password = "teste" });
        context.AuthData.Add(new AuthData() { LoginId = Guid.NewGuid(), Username = "ciro", Password = "challenge" });
        context.AuthData.Add(new AuthData() { LoginId = Guid.NewGuid(), Username = "author", Password = "myPass" });
        context.SaveChanges();
    }

}
