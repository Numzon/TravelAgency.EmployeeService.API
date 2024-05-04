namespace TravelAgency.EmployeeService.Domain.Entities;
public sealed class Salary : BaseAuditableEntity
{
    public required decimal Ammount { get; set; }
}
