using TravelAgency.EmployeeService.Domain.ValueObjects;

namespace TravelAgency.EmployeeService.Domain.Entities;
public sealed class Employee : BaseAuditableEntity
{
    public required string UserId { get; set; }
    public required int TravelAgencyId { get; set; }
    public required PersonalData PersonalData { get; set; }

    public required int SalaryId { get; set; }
    public Salary? Salary { get; set; }
}
