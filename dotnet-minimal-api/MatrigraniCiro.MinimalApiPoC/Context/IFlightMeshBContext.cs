using MatrigraniCiro.MinimalApiPoC.Domain;
using Microsoft.EntityFrameworkCore;

namespace MatrigraniCiro.MinimalApiPoC.Context;

public interface IFlightMeshBContext
{
    DbSet<FlightLeg> FlightLeg { get; }
}
