using System;

namespace MatrigraniCiro.DotNetCoreRestApiPoC.FlightLegs.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(Guid guid, Exception inner = null) : base(String.Format("Not found: {0}", guid), inner)
        { }
    }
}

