namespace TravelAgency.EmployeeService.Domain.Events;
public sealed class DriverEmployeeTypeCreatedEvent : INotification
{
    public required int EmployeeId { get; set; }
    public required string Identifier { get; set; }
    public required IEnumerable<int> CategoriesId { get; set; } 
}
