namespace TravelAgency.EmployeeService.Application.Common.Models;
public class EmployeeDto
{
    public required int Id { get; set; }
    public required string UserId { get; set; }
    public required int TravelAgencyId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }

    public required int SalaryId { get; set; }
}
