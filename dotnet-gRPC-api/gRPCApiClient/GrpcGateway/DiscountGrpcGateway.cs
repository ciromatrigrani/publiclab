using AutoMapper;
using gRPCDiscountAPI.Protos;

namespace gRPCApiClient.GrpcGateway
{
    public class DiscountGrpcGateway
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _grpcClient;
        private readonly IMapper _mapper;

        public DiscountGrpcGateway(DiscountProtoService.DiscountProtoServiceClient grpcClient, IMapper mapper)
        {
            _grpcClient = grpcClient ?? throw new ArgumentNullException(nameof(grpcClient));
            _mapper = mapper;
        }

        public async Task<DiscountModel> GetDiscount(string productName)
        {
            try
            {
                var dicountResponse = await _grpcClient.GetDiscountAsync(new GetDiscountRequest { ProductName = productName });

                var dicountModel = _mapper.Map<DiscountEntity, DiscountModel>(dicountResponse.Coupon);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }

        public async Task<DiscountModel> AddDiscount(DiscountEntity discountEntity)
        {
            var dicountResponse = await _grpcClient.PostDiscountAsync(new PostDiscountRequest { Coupon = discountEntity });
            return _mapper.Map<DiscountEntity, DiscountModel>(dicountResponse.Coupon);
        }
    }
}
