using MatrigraniCiro.MinimalApiPoC.Domain;

namespace MatrigraniCiro.MinimalApiPoC.Repository
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