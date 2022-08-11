using MatrigraniCiro.DotNetCoreRestApiPoC.FlightLegs.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MatrigraniCiro.DotNetCoreRestApiPoC.FlightLegs.Repository
{
    public interface IFlightLegRepository
    {
        Task<FlightLeg> AddFlightLeg(FlightLeg flightLeg, CancellationToken token);
        Task<bool> DeleteFlightLeg(Guid flightLegId, CancellationToken token);
        Task<FlightLeg> GetFlightLeg(Guid flightLegId, CancellationToken token);
        Task<IEnumerable<FlightLeg>> GetFlightLegs(CancellationToken token);
        Task<IEnumerable<FlightLeg>> GetFlightLegs(Guid company, CancellationToken token);
        Task<FlightLeg> UpdateFlightLeg(FlightLeg flightLeg, CancellationToken token);
    }
}