using AutoMapper;
using Grpc.Core;
using gRPCDiscountAPI.Domain;
using gRPCDiscountAPI.Exceptions;
using gRPCDiscountAPI.Protos;
using gRPCDiscountAPI.Repository;

namespace GrpcService1.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase, IDiscountService
    {
        private readonly ILogger<DiscountService> _logger;
        private readonly IDiscountRepository discountRepository;
        private readonly IMapper mapper;

        public DiscountService(ILogger<DiscountService> logger, IDiscountRepository discountRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.discountRepository = discountRepository ?? throw new ArgumentNullException(nameof(discountRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public override async Task<DiscountOkResponse> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await this.discountRepository.GetDiscount(request.ProductName);
            if (coupon == null) throw new RpcException(new Status(StatusCode.NotFound, $"Discount with productName {request.ProductName} not found."));
            var discountEntity = mapper.Map<DiscountModel, DiscountEntity>(coupon);
            return new DiscountOkResponse { Coupon = discountEntity };
        }

        public override async Task<DiscountCreatedResponse> PostDiscount(PostDiscountRequest request, ServerCallContext context)
        {
            var discountModel = mapper.Map<DiscountEntity, DiscountModel>(request.Coupon); ;
            var discountCreated = await this.discountRepository.CreateDiscount(discountModel);
            var discountEntity = mapper.Map<DiscountModel, DiscountEntity>(discountCreated);
            return new DiscountCreatedResponse { Coupon = discountEntity };
        }

        public override async Task<DiscountNoContentResponse> DelDiscount(DelDiscountRequest request, ServerCallContext context)
        {
            var deleted = await this.discountRepository.RemoveDiscount(request.ProductName);
            return new DiscountNoContentResponse { Done = deleted };
        }

        public override async Task<DiscountNoContentResponse> PutDiscount(PutDiscountRequest request, ServerCallContext context)
        {
            try
            {
                var discountModel = mapper.Map<DiscountEntity, DiscountModel>(request.Coupon);
                var updated = await this.discountRepository.UpdateDiscount(discountModel);
                return new DiscountNoContentResponse { Done = updated };
            }
            catch (NotFoundException)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with productName {request.Coupon.ProductName} not found."));
            }
        }

    }
}