using AutoMapper;
using MatrigraniCiro.DotNetCoreRestApiPoC.FlightLegs.Domain;
using MatrigraniCiro.DotNetCoreRestApiPoC.FlightLegs.Dto;

namespace MatrigraniCiro.DotNetCoreRestApiPoC.FlightLegs.Services
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            this.CreateMap<FlightLegRequest, FlightLeg>().ReverseMap();
            this.CreateMap<FlightLegResponse, FlightLeg>().ReverseMap();
        }
    }
}

