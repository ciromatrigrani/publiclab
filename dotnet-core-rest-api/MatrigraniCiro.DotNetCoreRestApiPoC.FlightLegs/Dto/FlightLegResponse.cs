using System;

namespace MatrigraniCiro.DotNetCoreRestApiPoC.FlightLegs.Dto
{
    public class FlightLegResponse
    {
        public Guid Id { get; set; }
        public string OriginIATA { get; set; }
        public string DestinyIATA { get; set; }
        public string AircraftRegistry { get; set; }
        public DateTime EstimatedDeparture { get; set; }
        public DateTime EstimatedArrival { get; set; }
        public DateTime? ExecutedDeparture { get; set; }
        public DateTime? ExecutedArrival { get; set; }
        public Guid CompanyId { get; set; }
        public bool Canceled { get; set; }
    }
}
