namespace TravelAgency.EmployeeService.Domain.ValueObjects;
public sealed class PersonalData
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
}
