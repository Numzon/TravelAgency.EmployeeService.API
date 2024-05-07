using Ardalis.Extensions.Serialization.Json;
using Microsoft.Extensions.DependencyInjection;
using TravelAgency.EmployeeService.Application.Common.Models;
using TravelAgency.SharedLibrary.RabbitMQ.Interfaces;

namespace TravelAgency.EmployeeService.Infrastructure.EventStrategies;
public class UserCreatedForEmployeeEventStrategy : IEventStrategy
{
    public async Task ExecuteEvent(IServiceScope scope, string message, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var employee = message.FromJson<UserForEmployeeCreatedPublishedDto>();

        Guard.Against.Null(employee, message: "Cannot be null");

        var repository = scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();

        await repository.UpdateUserIdAsync(employee.EmployeeId, employee.UserId, cancellationToken);
    }
}
