namespace MatrigraniCiro.MinimalApiPoC.Dto;

public class FlightLegRequest
{
    public string OriginIATA { get; set; }
    public string DestinyIATA { get; set; }
    public string AircraftRegistry { get; set; }
    public DateTime EstimatedDeparture { get; set; }
    public DateTime EstimatedArrival { get; set; }
    public Guid CompanyId { get; set; }
}
