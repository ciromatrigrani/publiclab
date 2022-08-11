using Newtonsoft.Json;
using System;

namespace MatrigraniCiro.DotNetCoreRestApiPoC.FlightLegs.Exceptions
{
    public class UnprocessableEntityException : Exception
    {
        public UnprocessableEntityException(Object entity, Exception inner = null) : base(JsonConvert.SerializeObject(entity), inner)
        { }
    }
}

