using TravelAgency.EmployeeService.Application.Employees.Commands;
using TravelAgency.EmployeeService.Domain.Entities;

namespace TravelAgency.EmployeeService.Application.Common.Interfaces;
public interface IEmployeeRepository
{
    Task UpdateUserIdAsync(int employeeId, string userId, CancellationToken cancellationToken);
    Task<Employee?> GetEmployeeAsync(int employeeId, CancellationToken cancellationToken);
    Task<int> CreateAsync(CreateEmployeeCommand request, CancellationToken cancellationToken);
}
