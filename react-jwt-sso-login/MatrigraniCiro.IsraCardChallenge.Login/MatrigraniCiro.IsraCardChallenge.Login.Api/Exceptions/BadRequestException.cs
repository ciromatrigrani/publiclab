using Newtonsoft.Json;

namespace MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Application.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException()
    { }

    public BadRequestException(Object request, Exception inner = null) : base(
        $"There's an problem with the request {JsonConvert.SerializeObject(request)}. Check the schema and try again.", inner)
    { }
}

