namespace TravelAgency.EmployeeService.Domain.Entities;
public sealed class DrivingLicence : BaseAuditableEntity
{
    public required string Identifier { get; set; }
}
