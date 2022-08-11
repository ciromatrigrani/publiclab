using System.ComponentModel.DataAnnotations;

namespace MatrigraniCiro.MinimalApiPoC.Domain;

public class FlightLeg
{
    [Key]
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
