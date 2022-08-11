using Grpc.Core;
using gRPCDiscountAPI.Protos;

namespace GrpcService1.Services
{
    public interface IDiscountService
    {
        Task<DiscountNoContentResponse> DelDiscount(DelDiscountRequest request, ServerCallContext context);
        Task<DiscountOkResponse> GetDiscount(GetDiscountRequest request, ServerCallContext context);
        Task<DiscountCreatedResponse> PostDiscount(PostDiscountRequest request, ServerCallContext context);
        Task<DiscountNoContentResponse> PutDiscount(PutDiscountRequest request, ServerCallContext context);
    }
}