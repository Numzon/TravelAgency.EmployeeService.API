namespace TravelAgency.EmployeeService.Application.Common.Interfaces;
public interface IEmployeePublisher
{
    Task PublishEmployeeCreated(string email, int employeeId, CancellationToken cancellationToken);
}
