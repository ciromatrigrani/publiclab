using AutoMapper;
using MatrigraniCiro.MinimalAPI.PaymentGateway.Services.Tests;
using MatrigraniCiro.MinimalApiPoC.Domain;
using MatrigraniCiro.MinimalApiPoC.Dto;
using MatrigraniCiro.MinimalApiPoC.Exceptions;
using MatrigraniCiro.MinimalApiPoC.Repository;
using MatrigraniCiro.MinimalApiPoC.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MatrigraniCiro.MinimalAPI.PaymentGateway.Tests.Services;

public class FlightLegServiceTests : IClassFixture<TestServicesFixture>
{
    private readonly TestServicesFixture fixture;
    private readonly IFlightLegService flightLegService;
    private readonly Mock<IFlightLegRepository> flightLegRepositoryMock;
    private readonly IMapper mapper;


    public FlightLegServiceTests(TestServicesFixture fixture)
    {
        this.fixture = fixture;
        this.flightLegRepositoryMock = this.fixture.FlightLegRepositoryMock;
        this.flightLegService = this.fixture.FlightLegService;
        this.mapper = this.fixture.Mapper;
    }

    [Fact]
    public async void TestGetFlightLegsSuccessReturnsList()
    {
        // arrange
        var flightLegsRes = new List<FlightLeg> { new FlightLeg() };
        this.flightLegRepositoryMock.Setup(r => r.GetFlightLegs(default)).ReturnsAsync(flightLegsRes);

        // act
        var flightLegs = await flightLegService.GetFlightLegs(default);

        // assert
        Assert.Equal(flightLegsRes.Count, flightLegs.Count());
    }

    [Fact]
    public async void TestGetFlightLegsSuccessReturnsEmptyList()
    {
        // arrange
        var flightLegsRes = new List<FlightLeg> { };
        this.flightLegRepositoryMock.Setup(r => r.GetFlightLegs(default)).ReturnsAsync(flightLegsRes);

        // act
        var flightLegs = await flightLegService.GetFlightLegs(default);

        // assert
        Assert.Empty(flightLegs);
    }

    [Fact]
    public async void TestGetFlightLegsByCompanySuccessReturnsList()
    {
        // arrange
        var company = Guid.NewGuid();
        var flightLegRes = new List<FlightLeg> { new FlightLeg() { CompanyId = company } };
        this.flightLegRepositoryMock.Setup(r => r.GetFlightLegs(company, default)).ReturnsAsync(flightLegRes);

        // act
        var flightLegs = await this.flightLegService.GetFlightLegs(company, default);

        // assert
        Assert.Equal(flightLegRes.Count, flightLegs.Count());
        Assert.Equal(flightLegRes.First().CompanyId, flightLegs.First().CompanyId);
    }

    [Fact]
    public async void TestGetFlightLegsByCompanySuccessReturnsEmptyList()
    {
        // arrange
        var company = Guid.NewGuid();
        var flightLegRes = new List<FlightLeg> { };
        this.flightLegRepositoryMock.Setup(r => r.GetFlightLegs(company, default)).ReturnsAsync(flightLegRes);

        // act
        var flightLegs = await this.flightLegService.GetFlightLegs(company, default);

        // assert
        Assert.Empty(flightLegs);
    }

    [Fact]
    public async void TestGetFlightLegSuccessReturnsFlightLegReponse()
    {
        // arrange
        var flightLegId = Guid.NewGuid();
        var flightLegRes = new FlightLeg() { Id = flightLegId };
        this.flightLegRepositoryMock.Setup(r => r.GetFlightLeg(flightLegId, default)).ReturnsAsync(flightLegRes);

        // act
        var flightLeg = await flightLegService.GetFlightLeg(flightLegId, default);

        // assert
        Assert.Equal(flightLegId, flightLeg.Id);
    }

    [Fact]
    public async void TestGetFlightLegFailureThrowsException()
    {
        // arrange
        this.flightLegRepositoryMock.Setup(r => r.GetFlightLeg(Guid.Empty, default)).Throws<Exception>();

        // act, assert
        await Assert.ThrowsAsync<Exception>(() => this.flightLegService.GetFlightLeg(Guid.Empty, default));
    }

    [Fact]
    public async void TestRegisterFlightLegSuccessReturnFlightLegReponse()
    {

        // arrange
        var flightLegId = Guid.NewGuid();
        var flightLegRes = new FlightLeg() { 
            Id = flightLegId, 
            AircraftRegistry = "AAA-111",
            CompanyId = Guid.NewGuid(),
            Canceled = false,
            DestinyIATA = "GRU",
            OriginIATA = "OPO",
            EstimatedDeparture = DateTime.UtcNow,
            EstimatedArrival = DateTime.UtcNow.AddHours(10),
            ExecutedDeparture = DateTime.UtcNow,
            ExecutedArrival = DateTime.UtcNow.AddHours(10)
        };
        var flightLegRequest = this.mapper.Map<FlightLegRequest>(flightLegRes);
        this.flightLegRepositoryMock.Setup(r => r.AddFlightLeg(It.IsAny<FlightLeg>(), default)).ReturnsAsync(flightLegRes);

        // act
        var flightLeg = await this.flightLegService.RegisterFlightLeg(flightLegId, flightLegRequest, default);

        // assert
        Assert.Equal(flightLegId, flightLeg.Id);
    }

    [Fact]
    public async void TestRegisterFlightLegFailureThrowsUnprocessableEntityException()
    {
        // act, assert
        await Assert.ThrowsAsync<UnprocessableEntityException>(() => this.flightLegService.RegisterFlightLeg(Guid.Empty, new FlightLegRequest(), default));
    }

    [Fact]
    public async void TestDeleteFlightLegSuccessReturnsTrue()
    {

        // arrange
        var flightLegId = Guid.NewGuid();
        this.flightLegRepositoryMock.Setup(r => r.DeleteFlightLeg(flightLegId, default)).ReturnsAsync(true);

        // act
        var response = await flightLegService.DeleteFlightLeg(flightLegId, default);

        // assert
        Assert.True(response);
    }

    [Fact]
    public async void TestDeleteFlightLegFailureReturnsFalse()
    {
        // arrange
        this.flightLegRepositoryMock.Setup(r => r.DeleteFlightLeg(Guid.Empty, default)).ReturnsAsync(false);

        // act
        var response = await flightLegService.DeleteFlightLeg(Guid.Empty, default);

        // assert
        Assert.False(response);
    }

    [Fact]
    public async void TestPutFlightLegSuccessReturnsFlightLegReponse()
    {
        // arrange
        var flightLegId = Guid.NewGuid();
        var flightLegRes = new FlightLeg()
        {
            Id = flightLegId,
            AircraftRegistry = "AAA-111",
            CompanyId = Guid.NewGuid(),
            Canceled = false,
            DestinyIATA = "GRU",
            OriginIATA = "OPO",
            EstimatedDeparture = DateTime.UtcNow,
            EstimatedArrival = DateTime.UtcNow.AddHours(10),
            ExecutedDeparture = DateTime.UtcNow,
            ExecutedArrival = DateTime.UtcNow.AddHours(10)
        };
        var flightLegRequest = this.mapper.Map<FlightLegRequest>(flightLegRes);
        this.flightLegRepositoryMock.Setup(r => r.UpdateFlightLeg(It.IsAny<FlightLeg>(), default)).ReturnsAsync(flightLegRes);

        // act
        var flightLegResponse = await flightLegService.PutFlightLeg(flightLegId, flightLegRequest, default);

        // assert
        Assert.Equal(flightLegId, flightLegResponse.Id);
    }

    [Fact]
    public async void TestPutFlightLegFailureThrowsUnprocessableEntityException()
    {
        // act, assert
        await Assert.ThrowsAsync<UnprocessableEntityException>(() => this.flightLegService.PutFlightLeg(Guid.Empty, new FlightLegRequest(), default));
    }

    [Fact]
    public async void TestPatchFlightLegSuccessReturnsFlightLegReponse()
    {
        // arrange
        var flightLegId = Guid.NewGuid();
        var expectedFlightLegNumber = "987654321";
        var patchEntity = new JsonPatchDocument<FlightLegRequest>();
        patchEntity.Operations.Add(new Operation<FlightLegRequest>(op: "replace", path: "/Number", null, value: expectedFlightLegNumber));
        var flightLegRes = new FlightLeg()
        {
            Id = flightLegId,
            AircraftRegistry = "AAA-111",
            CompanyId = Guid.NewGuid(),
            Canceled = false,
            DestinyIATA = "GRU",
            OriginIATA = "OPO",
            EstimatedDeparture = DateTime.UtcNow,
            EstimatedArrival = DateTime.UtcNow.AddHours(10),
            ExecutedDeparture = DateTime.UtcNow,
            ExecutedArrival = DateTime.UtcNow.AddHours(10)
        };
        this.flightLegRepositoryMock.Setup(r => r.GetFlightLeg(flightLegId, default)).ReturnsAsync(flightLegRes);
        this.flightLegRepositoryMock.Setup(r => r.UpdateFlightLeg(It.IsAny<FlightLeg>(), default)).ReturnsAsync(flightLegRes);

        // act
        var flightLegResponse = await flightLegService.PatchFlightLeg(flightLegId, patchEntity, default);

        // assert
        Assert.Equal(flightLegId, flightLegResponse.Id);
    }

    [Fact]
    public async void TestPatchFlightLegFailureThrowsNotFoundException()
    {
        // arrange
        this.flightLegRepositoryMock.Setup(r => r.GetFlightLeg(Guid.Empty, default)).Throws<NullReferenceException>();

        // act, assert
        await Assert.ThrowsAsync<NotFoundException>(() => this.flightLegService.PatchFlightLeg(Guid.Empty, new JsonPatchDocument<FlightLegRequest>(), default));
    }

    [Fact]
    public async void TestPatchFlightLegFailureThrowsBadRequestException()
    {
        // arrange
        this.flightLegRepositoryMock.Setup(r => r.GetFlightLeg(Guid.Empty, default)).ReturnsAsync(new FlightLeg());
        var patchEntity = new JsonPatchDocument<FlightLegRequest>();
        patchEntity.Operations.Add(new Operation<FlightLegRequest>(op: "x", path: "/y", null, value: "z"));

        // act, assert
        await Assert.ThrowsAsync<BadRequestException>(() => this.flightLegService.PatchFlightLeg(Guid.Empty, patchEntity, default));
    }
}

