using AutoMapper;
using MatrigraniCiro.DotNetCoreRestApiPoC.FlightLegs.Repository;
using Moq;
using System;

namespace MatrigraniCiro.DotNetCoreRestApiPoC.FlightLegs.Services.Tests
{
    public class TestServicesFixture : IDisposable
    {
        public Mock<IFlightLegService> FlightLegServiceMock { get; }
        public Mock<IFlightLegRepository> FlightLegRepositoryMock { get; }
        public IFlightLegService FlightLegService { get; }
        public IMapper Mapper { get; }
        public IFlightLegRepository FlightLegRepository { get; }

        public TestServicesFixture()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new Mapping()));
            this.Mapper = mapperConfig.CreateMapper();
            this.FlightLegRepositoryMock = new Mock<IFlightLegRepository>();
            this.FlightLegService = new FlightLegService(this.FlightLegRepositoryMock.Object, this.Mapper);
        }

        public void Dispose()
        { }
    }
}