using MatrigraniCiro.DotNetCoreRestApiPoC.FlightLegs.Domain;
using Microsoft.EntityFrameworkCore;

namespace MatrigraniCiro.DotNetCoreRestApiPoC.FlightLegs.Context
{
    public class FlightMeshDBContext : DbContext, IFlightMeshBContext
    {
        public FlightMeshDBContext(DbContextOptions<FlightMeshDBContext> options) : base(options) { }

        public DbSet<FlightLeg> FlightLeg { get; }
    }
}