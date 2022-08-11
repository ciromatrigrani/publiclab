using AutoMapper;
using MatrigraniCiro.MinimalApiPoC.Domain;
using MatrigraniCiro.MinimalApiPoC.Dto;
using MatrigraniCiro.MinimalApiPoC.Exceptions;
using MatrigraniCiro.MinimalApiPoC.Repository;
using MatrigraniCiro.MinimalApiPoC.Services.Helpers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Exceptions;

namespace MatrigraniCiro.MinimalApiPoC.Services;

public class FlightLegService : IFlightLegService
{
    private readonly IFlightLegRepository flightLegRepository;
    private readonly IMapper mapper;

    public FlightLegService(IFlightLegRepository flightLegRepository, IMapper mapper)
    {
        this.flightLegRepository = flightLegRepository;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<FlightLegResponse>> GetFlightLegs(CancellationToken token = default)
    {
        return mapper.Map<IEnumerable<FlightLegResponse>>(await this.flightLegRepository.GetFlightLegs(token)); ;
    }

    public async Task<IEnumerable<FlightLegResponse>> GetFlightLegs(Guid company, CancellationToken token = default)
    {
        var flightLegs = await this.flightLegRepository.GetFlightLegs(company, token);
        var flightLegDtos = mapper.Map<IEnumerable<FlightLegResponse>>(flightLegs);

        return flightLegDtos;
    }

    public async Task<FlightLegResponse> GetFlightLeg(Guid flightLegId, CancellationToken token = default)
    {
        return mapper.Map<FlightLegResponse>(await this.flightLegRepository.GetFlightLeg(flightLegId, token));
    }

    public async Task<FlightLegResponse> RegisterFlightLeg(Guid newFlightLegId, FlightLegRequest flightLegRequest, CancellationToken token = default)
    {
        if (Guard.IsAnyPropertyNull(flightLegRequest))
            throw new UnprocessableEntityException(flightLegRequest);
        var flightLeg = mapper.Map<FlightLeg>(flightLegRequest);
        flightLeg.Id = newFlightLegId;
        return mapper.Map<FlightLegResponse>(await this.flightLegRepository.AddFlightLeg(flightLeg, token));
    }

    public Task<bool> DeleteFlightLeg(Guid flightLegId, CancellationToken token = default)
    {
        return this.flightLegRepository.DeleteFlightLeg(flightLegId, token);
    }

    public async Task<FlightLegResponse> PutFlightLeg(Guid flightLegId, FlightLegRequest flightLegRequest, CancellationToken token = default)
    {
        if (Guard.IsAnyPropertyNull(flightLegRequest))
            throw new UnprocessableEntityException(flightLegId);
        var flightLeg = mapper.Map<FlightLeg>(flightLegRequest);
        flightLeg.Id = flightLegId;
        return mapper.Map<FlightLegResponse>(await this.flightLegRepository.UpdateFlightLeg(flightLeg, token));
    }

    public async Task<FlightLegResponse> PatchFlightLeg(Guid flightLegId, JsonPatchDocument<FlightLegRequest> flightLegPatchRequest, CancellationToken token = default)
    {
        try
        {
            var flightLeg = await this.flightLegRepository.GetFlightLeg(flightLegId, token);

            var flightLegRequest = mapper.Map<FlightLegRequest>(flightLeg);
            flightLegPatchRequest.ApplyTo(flightLegRequest);
            flightLeg = mapper.Map<FlightLeg>(flightLegRequest);
            flightLeg.Id = flightLegId;
            flightLeg = await this.flightLegRepository.UpdateFlightLeg(flightLeg, token);
            return mapper.Map<FlightLegResponse>(flightLeg);
        }
        catch (JsonPatchException ex) { throw new BadRequestException(flightLegPatchRequest, ex); }
        catch (Exception ex) { throw new NotFoundException(flightLegId, ex); }
    }
}
