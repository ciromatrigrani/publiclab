using MatrigraniCiro.DotNetCoreRestApiPoC.FlightLegs.Domain;
using Microsoft.EntityFrameworkCore;

namespace MatrigraniCiro.DotNetCoreRestApiPoC.FlightLegs.Context
{
    public interface IFlightMeshBContext
    {
        DbSet<FlightLeg> FlightLeg { get; }
    }
}
