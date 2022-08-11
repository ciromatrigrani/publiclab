using Newtonsoft.Json;

namespace MatrigraniCiro.MinimalApiPoC.Exceptions;

public class UnprocessableEntityException : Exception
{
    public UnprocessableEntityException(Object entity, Exception inner = null) : base(JsonConvert.SerializeObject(entity), inner)
    { }
}
