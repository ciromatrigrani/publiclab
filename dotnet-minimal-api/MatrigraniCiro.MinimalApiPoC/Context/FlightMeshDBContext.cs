using MatrigraniCiro.MinimalApiPoC.Domain;
using Microsoft.EntityFrameworkCore;

namespace MatrigraniCiro.MinimalApiPoC.Context;

public class FlightMeshBContext : DbContext, IFlightMeshBContext
{
    public FlightMeshBContext(DbContextOptions<FlightMeshBContext> options) : base(options) { }

    public DbSet<FlightLeg> FlightLeg { get; }
}
