using Ardalis.Extensions.Serialization.Json;
using TravelAgency.EmployeeService.Application.Common.Interfaces;
using TravelAgency.EmployeeService.Application.Common.Models;
using TravelAgency.SharedLibrary.Enums;
using TravelAgency.SharedLibrary.RabbitMQ.Interfaces;

namespace TravelAgency.EmployeeService.Infrastructure.Publishers;
public sealed class EmployeePublisher : IEmployeePublisher
{
    private readonly IMessageBusPublisher _publisher;

    public EmployeePublisher(IMessageBusPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task PublishEmployeeCreated(string email, int employeeId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var model = new EmployeeCreatedPublishedDto { EmployeeId = employeeId, Email = email, Event = EventTypes.EmployeeCreated };
        var message = model.ToJson();
        await _publisher.Publish(message);
    }
}
