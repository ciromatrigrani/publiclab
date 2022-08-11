using AutoMapper;
using MatrigraniCiro.MinimalApiPoC.Domain;
using MatrigraniCiro.MinimalApiPoC.Dto;

namespace MatrigraniCiro.MinimalApiPoC.Services;

public class Mapping : Profile
{
    public Mapping()
    {
        this.CreateMap<FlightLegRequest, FlightLeg>().ReverseMap();
        this.CreateMap<FlightLegResponse, FlightLeg>().ReverseMap();
    }
}
