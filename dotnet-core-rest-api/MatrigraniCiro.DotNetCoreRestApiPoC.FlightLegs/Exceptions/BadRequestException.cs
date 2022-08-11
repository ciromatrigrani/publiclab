using Newtonsoft.Json;
using System;

namespace MatrigraniCiro.DotNetCoreRestApiPoC.FlightLegs.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(Object request, Exception inner = null) : base(JsonConvert.SerializeObject(request), inner)
        { }
    }
}

