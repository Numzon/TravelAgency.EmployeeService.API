namespace TravelAgency.EmployeeService.Domain.Entities;
public sealed class Driver : BaseAuditableEntity
{
    public required int EmployeeId { get; set; }
    public required Employee Employee { get; set; }

    public required int DrivingLicenceId { get; set; }
    public required DrivingLicence DrivingLicence { get; set; }
}
