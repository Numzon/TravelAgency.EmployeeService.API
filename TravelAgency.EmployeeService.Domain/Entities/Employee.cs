using TravelAgency.EmployeeService.Domain.Enums;
using TravelAgency.EmployeeService.Domain.ValueObjects;

namespace TravelAgency.EmployeeService.Domain.Entities;
public sealed class Employee : BaseAuditableEntity
{
    public required string UserId { get; set; }
    public required int TravelAgencyId { get; set; }
    public required PersonalData PersonalData { get; set; }
    public required EmployeeTypes EmployeeType { get; set; }

    public string? CustomEmployeeType { get; set; }

    public int? DrivingLicenceId { get; set; }
    public DrivingLicence? DrivingLicence { get; set; }

    public required int SalaryId { get; set; }
    public Salary? Salary { get; set; }
}
