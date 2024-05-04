namespace TravelAgency.EmployeeService.Domain.Entities;
public sealed class DrivingLicenceCategory : BaseAuditableEntity
{
    public required int DrivingLicenceId { get; set; }
    public required DrivingLicence DrivingLicence { get; set; }

    public required int CategoryId { get; set; }
    public required Category Category { get; set; }
}
