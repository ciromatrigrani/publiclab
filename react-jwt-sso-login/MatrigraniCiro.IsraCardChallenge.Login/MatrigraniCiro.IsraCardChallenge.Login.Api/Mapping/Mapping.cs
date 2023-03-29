using AutoMapper;
using MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Application.Dto;
using MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Model;

namespace MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Application.Mapping;

public class Mapping : Profile
{
    public Mapping()
    {
        this.CreateMap<LoginRequest, AuthDataResponse>().ReverseMap();
        this.CreateMap<AuthDataResponse, AuthData>().ReverseMap();
        this.CreateMap<AuthData, LoginRequest>().ReverseMap();
    }
}

