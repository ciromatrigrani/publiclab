using Newtonsoft.Json;

namespace MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Application.Exceptions;

public class UnprocessableEntityException : Exception
{
    public UnprocessableEntityException() 
    { }

    public UnprocessableEntityException(Object entity, Exception inner = null) : base(
        $"The system cannot process the entity {JsonConvert.SerializeObject(entity)}. Please, check the properties and try again.", inner)
    { }
}

