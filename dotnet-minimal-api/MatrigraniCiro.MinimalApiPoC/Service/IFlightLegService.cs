using MatrigraniCiro.MinimalApiPoC.Dto;
using Microsoft.AspNetCore.JsonPatch;

namespace MatrigraniCiro.MinimalApiPoC.Services
{
    public interface IFlightLegService
    {
        Task<bool> DeleteFlightLeg(Guid flightLegId, CancellationToken token = default);
        Task<FlightLegResponse> GetFlightLeg(Guid flightLegId, CancellationToken token = default);
        Task<IEnumerable<FlightLegResponse>> GetFlightLegs(CancellationToken token = default);
        Task<IEnumerable<FlightLegResponse>> GetFlightLegs(Guid company, CancellationToken token = default);
        Task<FlightLegResponse> PatchFlightLeg(Guid flightLegId, JsonPatchDocument<FlightLegRequest> flightLegPatchRequest, CancellationToken token = default);
        Task<FlightLegResponse> PutFlightLeg(Guid flightLegId, FlightLegRequest flightLegRequest, CancellationToken token = default);
        Task<FlightLegResponse> RegisterFlightLeg(Guid newFlightLegId, FlightLegRequest flightLegRequest, CancellationToken token = default);
    }
}