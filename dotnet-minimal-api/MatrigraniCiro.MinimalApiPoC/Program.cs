using MatrigraniCiro.MinimalApiPoC.Context;
using MatrigraniCiro.MinimalApiPoC.Dto;
using MatrigraniCiro.MinimalApiPoC.Exceptions;
using MatrigraniCiro.MinimalApiPoC.Repository;
using MatrigraniCiro.MinimalApiPoC.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

/*
 * General config
 */

var i = 1;
Console.WriteLine("###############################");
Console.WriteLine(i++);
Console.WriteLine("###############################");


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FlightMeshBContext>(opt => opt.UseInMemoryDatabase("flight_mesh_db"));
builder.Services.AddAutoMapper(mapper => mapper.AddMaps(typeof(Mapping).Assembly));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo
{
    Version = "v1",
    Title = "PoC Minimal API for Flight Mesh control",
    Description = "A dot NET 6 Minimal API PoC made by Ciro Matrigrani",
    Contact = new OpenApiContact
    {
        Name = "Ciro Matrigrani",
        Email = "ciromatrigrani@gmail.com",
        Url = new Uri("https://sites.google.com/site/cmatripgita/")
    }
}));

/*
 * Services Injection
 */

builder.Services.AddTransient<IFlightLegService, FlightLegService>();
builder.Services.AddTransient<IFlightLegRepository, FlightLegRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/*
 * Dot NET 6 Minimal API FlightLeg Controller
 */

app.MapGet("FlightLeg", async ([FromServices] IFlightLegService flightLegService) => Results.Ok(await flightLegService.GetFlightLegs()));

app.MapGet("FlightLeg/{Company}", async ([FromQuery] Guid companyId, [FromServices] IFlightLegService flightLegService) =>
{
    try
    {
        var flightLegReponse = await flightLegService.GetFlightLegs(companyId);
        return Results.Ok(flightLegReponse);
    }
    catch { return Results.NotFound(companyId); }
});

app.MapGet("FlightLeg/{FlightLegId}", async ([FromQuery] Guid flightLegId, [FromServices] IFlightLegService flightLegService) =>
{
    try
    {
        var flightLegReponse = await flightLegService.GetFlightLeg(flightLegId);
        return Results.Ok(flightLegReponse);
    }
    catch { return Results.NotFound(flightLegId); }
});

/// <summary>
/// <see cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/201"/>
/// <seealso cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/422"/>
/// </summary>
app.MapPost("FlightLeg",
    [ProducesResponseType(StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
async ([FromBody] FlightLegRequest flightLeg, [FromServices] IFlightLegService flightLegService) =>
    {
        try
        {
            var newFlightLegId = Guid.NewGuid();
            return Results.Created(newFlightLegId.ToString(), await flightLegService.RegisterFlightLeg(newFlightLegId, flightLeg));
        }
        catch { return Results.UnprocessableEntity(); }
    });

/// <summary>
/// <see cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/204"/>
/// </summary>
app.MapDelete("FlightLeg/{FlightLegId}",
    [ProducesResponseType(StatusCodes.Status204NoContent)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
async ([FromQuery] Guid flightLegId, [FromServices] IFlightLegService flightLegService) =>
        await flightLegService.DeleteFlightLeg(flightLegId) ? Results.NoContent() : Results.NotFound(flightLegId));

/// <summary>
/// <see cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/204"/>
/// <seealso cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/409"/>
/// </summary>
app.MapPut("FlightLeg/{flightLegId}/",
    [ProducesResponseType(StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status204NoContent)]
[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
[ProducesResponseType(StatusCodes.Status409Conflict)]
async ([FromQuery] Guid flightLegId, [FromBody] FlightLegRequest flightLegRequest, [FromServices] IFlightLegService flightLegService) =>
    {
        try
        {
            var flightLegResponse = await flightLegService.PutFlightLeg(flightLegId, flightLegRequest);
            return Results.NoContent();
        }
        catch (NotFoundException)
        {
            return Results.Created(flightLegId.ToString(), flightLegRequest);
        }
        catch (UnprocessableEntityException)
        {
            return Results.UnprocessableEntity(flightLegRequest);
        }
        catch
        {
            return Results.Conflict();
        }
    });

/// <summary>
/// <see cref="https://code-maze.com/using-httpclient-to-send-http-patch-requests-in-asp-net-core/"/>
/// </summary>
app.MapMethods("FlightLeg/{flightLegId}/", new List<string> { "PATCH" },
    [ProducesResponseType(StatusCodes.Status204NoContent)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
async ([FromQuery] Guid flightLegId, [FromBody] JsonPatchDocument<FlightLegRequest> flightLegPatchRequest, [FromServices] IFlightLegService flightLegService) =>
    {
        try
        {
            var flightLeg = await flightLegService.PatchFlightLeg(flightLegId, flightLegPatchRequest);
            return Results.NoContent();
        }
        catch (NotFoundException)
        {
            return Results.NotFound(flightLegId);
        }
        catch (BadRequestException)
        {
            return Results.BadRequest(flightLegPatchRequest);
        }
    });


app.Run();