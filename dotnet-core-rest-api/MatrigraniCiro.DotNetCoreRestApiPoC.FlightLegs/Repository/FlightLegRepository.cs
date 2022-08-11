using MatrigraniCiro.DotNetCoreRestApiPoC.FlightLegs.Context;
using MatrigraniCiro.DotNetCoreRestApiPoC.FlightLegs.Domain;
using MatrigraniCiro.DotNetCoreRestApiPoC.FlightLegs.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace MatrigraniCiro.DotNetCoreRestApiPoC.FlightLegs.Repository
{

    public class FlightLegRepository : IFlightLegRepository
    {
        private readonly FlightMeshDBContext Context;

        public FlightLegRepository(FlightMeshDBContext context)
        {
            this.Context = context;
        }

        public async Task<FlightLeg> AddFlightLeg(FlightLeg flightLeg, CancellationToken token)
        {
            Context.FlightLeg.Add(flightLeg);
            await ((DbContext)Context).SaveChangesAsync(token);
            return await GetFlightLeg(flightLeg.Id, token);
        }

        public async Task<IEnumerable<FlightLeg>> GetFlightLegs(Guid company, CancellationToken token)
        {
            return await Context.FlightLeg.Where(c => c.CompanyId.Equals(company)).ToListAsync(token);
        }

        public async Task<FlightLeg> GetFlightLeg(Guid flightLegId, CancellationToken token)
        {
            return await Context.FlightLeg.Where(c => c.Id == flightLegId).FirstAsync(token);
        }

        public async Task<IEnumerable<FlightLeg>> GetFlightLegs(CancellationToken token)
        {
            return await Context.FlightLeg.ToListAsync(token);
        }

        public async Task<bool> DeleteFlightLeg(Guid flightLegId, CancellationToken token)
        {
            Context.FlightLeg.Remove(new FlightLeg { Id = flightLegId });
            return (await ((DbContext)Context).SaveChangesAsync(token) > 0);
        }

        public async Task<FlightLeg> UpdateFlightLeg(FlightLeg flightLeg, CancellationToken token)
        {
            try
            {
                await Context.FlightLeg.Where(c => c.Id == flightLeg.Id).FirstAsync(token);
                Context.FlightLeg.Update(flightLeg);
            }
            catch (Exception ex)
            {
                Context.FlightLeg.Add(flightLeg);
                throw new NotFoundException(flightLeg.Id, ex);
            }
            finally
            {
                await ((DbContext)Context).SaveChangesAsync(token);
            }
            return flightLeg;
        }
    }

}