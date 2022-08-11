namespace MatrigraniCiro.MinimalApiPoC.Services.Helpers;

public class Guard
{
    public static bool IsAnyPropertyNull(Object myObject)
    {
        return myObject.GetType().GetProperties()
            .Select(pi => pi.GetValue(myObject))
            .Any(value => value is null);
    }
}
