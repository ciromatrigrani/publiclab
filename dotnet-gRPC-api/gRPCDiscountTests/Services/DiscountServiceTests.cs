using AutoMapper;
using gRPCDiscountAPI.Domain;
using gRPCDiscountAPI.Repository;
using GrpcService1.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using gRPCDiscountAPI.Protos;
using gRPCDiscountAPI.Exceptions;

namespace gRPCDiscountTests.Services;

public class DiscountServiceTests : IClassFixture<TestServicesFixture>
{
    private readonly TestServicesFixture fixture;
    private readonly IDiscountService discountService;
    private readonly Mock<IDiscountRepository> discountRepositoryMock;
    private readonly IMapper mapper;


    public DiscountServiceTests(TestServicesFixture fixture)
    {
        this.fixture = fixture;
        this.discountRepositoryMock = this.fixture.DiscountRepositoryMock;
        this.discountService = this.fixture.DiscountService;
        this.mapper = this.fixture.Mapper;
    }

    [Fact]
    public async void TestGetDiscountsSuccessReturnsList()
    {
        // arrange
        var couponRes = new DiscountModel { ProductName = "prd1" };
        this.discountRepositoryMock.Setup(r => r.GetDiscount(default)).ReturnsAsync(couponRes);
        var request = new GetDiscountRequest { ProductName = "prd1" };

        // act
        var coupon = await discountService.GetDiscount(request, default);

        // assert
        Assert.Equal(couponRes.ProductName, coupon.Coupon.ProductName);
    }

    [Fact]
    public async void TestGetDiscountFailureThrowsException()
    {
        // arrange
        this.discountRepositoryMock.Setup(r => r.GetDiscount(string.Empty)).Throws<Exception>();
        var request = new GetDiscountRequest { ProductName = string.Empty };

        // act, assert
        await Assert.ThrowsAsync<Exception>(() => this.discountService.GetDiscount(request, default));
    }

    [Fact]
    public async void TestPostDiscountSuccessReturnDiscountReponse()
    {
        // arrange
        var discountRes = new DiscountModel() { Id = 1, ProductName = "prd1", Amount = 4, Description = "test" };
        this.discountRepositoryMock.Setup(r => r.CreateDiscount(It.IsAny<DiscountModel>())).ReturnsAsync(discountRes);
        var discountReq = new PostDiscountRequest { Coupon = mapper.Map<DiscountModel, DiscountEntity>(discountRes) };

        // act
        var discount = await this.discountService.PostDiscount(discountReq, default);

        // assert
        Assert.Equal("prd1", discount.Coupon.ProductName);
    }

    [Fact]
    public async void TestDeleteDiscountSuccessReturnsTrue()
    {

        // arrange
        var productName = "prd1";
        this.discountRepositoryMock.Setup(r => r.RemoveDiscount(productName)).ReturnsAsync(true);
        var discountReq = new DelDiscountRequest { ProductName = productName };

        // act
        var response = await discountService.DelDiscount(discountReq, default);

        // assert
        Assert.True(response.Done);
    }

    [Fact]
    public async void TestDeleteDiscountFailureReturnsFalse()
    {
        // arrange
        this.discountRepositoryMock.Setup(r => r.RemoveDiscount(string.Empty)).ReturnsAsync(false);
        var discountReq = new DelDiscountRequest { ProductName = string.Empty };

        // act
        var response = await discountService.DelDiscount(discountReq, default);

        // assert
        Assert.False(response.Done);
    }

    [Fact]
    public async void TestPutDiscountSuccessReturnsDiscountReponse()
    {
        // arrange
        var discountRes = new DiscountModel() { Id = 1, ProductName = "prd1", Amount = 4, Description = "test" };
        var discountReq = new PutDiscountRequest { Coupon = mapper.Map<DiscountModel, DiscountEntity>(discountRes) };
        this.discountRepositoryMock.Setup(r => r.UpdateDiscount(It.IsAny<DiscountModel>())).ReturnsAsync(true);

        // act
        var discountResponse = await discountService.PutDiscount(discountReq, default);

        // assert
        Assert.True(discountResponse.Done);
    }

    [Fact]
    public async void TestPutDiscountFailureThrowsNotFoundException()
    {
        var discountReq = new PutDiscountRequest { Coupon = new DiscountEntity { ProductName = string.Empty } };
        this.discountRepositoryMock.Setup(r => r.UpdateDiscount(It.IsAny<DiscountModel>())).Throws<NotFoundException>(default);

        // act, assert
        await Assert.ThrowsAsync<NotFoundException>(() => this.discountService.PutDiscount(discountReq, default));
    }
}

