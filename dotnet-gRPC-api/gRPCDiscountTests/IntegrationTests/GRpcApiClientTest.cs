using gRPCApiClient.GrpcGateway;
using gRPCDiscountAPI.Protos;
using System;
using System.Threading.Tasks;
using Xunit;

namespace gRPCDiscountTests.IntegrationTests
{
    public class GRpcApiClientTest
    {
        private readonly DiscountGrpcGateway discountGrpcGateway;

        public GRpcApiClientTest(DiscountGrpcGateway discountGrpcGateway)
        {
            this.discountGrpcGateway = discountGrpcGateway ?? throw new ArgumentNullException(nameof(discountGrpcGateway));
        }

        [Fact]
        public async Task DiscountGrpcGatewayPostAndGetDiscountSuccess()
        {
            var discountEntity = new DiscountEntity { ProductName = "prd1", Id = 1, Amount = 4, Description = "test" };
            this.discountGrpcGateway.AddDiscount(discountEntity);
            var discountReponse = await this.discountGrpcGateway.GetDiscount("prd1");

            Assert.Equal("prd1", discountReponse.ProductName);
        }
    }
}