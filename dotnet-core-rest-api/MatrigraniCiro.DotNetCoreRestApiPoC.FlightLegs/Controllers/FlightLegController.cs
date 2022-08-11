using MatrigraniCiro.DotNetCoreRestApiPoC.FlightLegs.Dto;
using MatrigraniCiro.DotNetCoreRestApiPoC.FlightLegs.Exceptions;
using MatrigraniCiro.DotNetCoreRestApiPoC.FlightLegs.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MatrigraniCiro.DotNetCoreRestApiPoC.FlightLegs.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class FlightLegController : Controller
    {
        private readonly IFlightLegService flightLegService;

        public FlightLegController(IFlightLegService flightLegService)
        {
            this.flightLegService = flightLegService;
        }

        [HttpGet("{CompanyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFlightLegsByCompany([FromQuery] Guid companyId, CancellationToken token = default)
        {
            try
            {
                var flightLegReponse = await flightLegService.GetFlightLegs(companyId, token);
                return Ok(flightLegReponse);
            }
            catch { return NotFound(companyId); }
        }

        [HttpGet("{FlightLegId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFlightLeg([FromQuery] Guid flightLegId, CancellationToken token = default)
        {
            try
            {
                var flightLegReponse = await flightLegService.GetFlightLeg(flightLegId, token);
                return Ok(flightLegReponse);
            }
            catch { return NotFound(flightLegId); }
        }

        /// <summary>
        /// <see cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/204"/>
        /// </summary>
        [HttpDelete("{FlightLegId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromQuery] Guid flightLegId, CancellationToken token = default)
        {
            try
            {
                await flightLegService.DeleteFlightLeg(flightLegId, token);
                return NoContent();
            }
            catch { return NotFound(flightLegId); }
        }

        /// <summary>
        /// <see cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/201"/>
        /// <seealso cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/422"/>
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Post([FromBody] FlightLegRequest flightLeg, CancellationToken token = default)
        {
            try
            {
                var newFlightLegId = Guid.NewGuid();
                return Created(newFlightLegId.ToString(), await flightLegService.RegisterFlightLeg(newFlightLegId, flightLeg));
            }
            catch { return UnprocessableEntity(); }
        }

        /// <summary>
        /// <see cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/201"/>
        /// <seealso cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/422"/>
        /// </summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Put([FromQuery] Guid flightLegId, [FromBody] FlightLegRequest flightLegRequest, CancellationToken token = default)
        {
            try
            {
                var flightLegResponse = await flightLegService.PutFlightLeg(flightLegId, flightLegRequest);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return Created(flightLegId.ToString(), flightLegRequest);
            }
            catch (UnprocessableEntityException)
            {
                return UnprocessableEntity(flightLegRequest);
            }
            catch
            {
                return Conflict();
            }
        }

        /// <summary>
        /// <see cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/201"/>
        /// <seealso cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/422"/>
        /// </summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Patch([FromQuery] Guid flightLegId, [FromBody] JsonPatchDocument<FlightLegRequest> flightLegPatchRequest, CancellationToken token = default)
        {
            try
            {
                var flightLeg = await flightLegService.PatchFlightLeg(flightLegId, flightLegPatchRequest, token);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound(flightLegId);
            }
            catch (BadRequestException)
            {
                return BadRequest(flightLegPatchRequest);
            }
        }
    }
}

