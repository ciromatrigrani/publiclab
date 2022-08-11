using AutoMapper;
using gRPCDiscountAPI.Mapper;
using gRPCDiscountAPI.Repository;
using GrpcService1.Services;
using Moq;
using System;

namespace gRPCDiscountTests.Services;

public class TestServicesFixture : IDisposable
{
    public Mock<IDiscountService> DiscountServiceMock { get; }
    public Mock<IDiscountRepository> DiscountRepositoryMock { get; }
    public IDiscountService DiscountService { get; }
    public IMapper Mapper { get; }
    public IDiscountRepository DiscountRepository { get; }

    public TestServicesFixture()
    {
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new DiscountProfile()));
        this.Mapper = mapperConfig.CreateMapper();
        this.DiscountRepositoryMock = new Mock<IDiscountRepository>();
        this.DiscountService = new DiscountService(null, this.DiscountRepositoryMock.Object, this.Mapper);
    }

    public void Dispose()
    {
        
    }
}
