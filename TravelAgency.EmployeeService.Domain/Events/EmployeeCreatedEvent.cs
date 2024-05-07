namespace TravelAgency.EmployeeService.Domain.Events;
public sealed class EmployeeCreatedEvent : INotification
{
    public required int EmployeeId { get; set; }
    public required string Email { get; set; }
}
