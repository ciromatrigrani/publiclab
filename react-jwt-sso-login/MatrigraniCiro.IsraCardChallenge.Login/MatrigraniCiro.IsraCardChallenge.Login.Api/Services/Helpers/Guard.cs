using MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Application.Dto;
using MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Application.Exceptions;

namespace MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Application.Services.Helpers;

public class Guard
{
    public static bool IsAnyPropertyNull(Object myObject)
    {
        return myObject.GetType().GetProperties()
            .Select(pi => pi.GetValue(myObject))
            .Any(value => value is null);
    }
}
