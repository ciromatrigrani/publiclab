using AutoMapper;
using gRPCDiscountAPI.Protos;

namespace gRPCApiClient.GrpcGateway
{
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            CreateMap<DiscountEntity, DiscountModel>().ReverseMap();
        }
    }
}
