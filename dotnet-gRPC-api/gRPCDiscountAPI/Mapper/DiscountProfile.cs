using AutoMapper;
using gRPCDiscountAPI.Domain;
using gRPCDiscountAPI.Protos;

namespace gRPCDiscountAPI.Mapper
{
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            CreateMap<DiscountEntity, DiscountModel>().ReverseMap();
        }
    }
}
