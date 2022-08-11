using Newtonsoft.Json;

namespace MatrigraniCiro.MinimalApiPoC.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(Object request, Exception inner = null) : base(JsonConvert.SerializeObject(request), inner)
    { }
}
