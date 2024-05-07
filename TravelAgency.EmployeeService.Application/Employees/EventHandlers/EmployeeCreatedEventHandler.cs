using TravelAgency.EmployeeService.Application.Common.Interfaces;
using TravelAgency.EmployeeService.Domain.Events;

namespace TravelAgency.EmployeeService.Application.Employees.EventHandlers;
public sealed class EmployeeCreatedEventHandler : INotificationHandler<EmployeeCreatedEvent>
{
    private readonly IEmployeePublisher _publisher;

    public EmployeeCreatedEventHandler(IEmployeePublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task Handle(EmployeeCreatedEvent notification, CancellationToken cancellationToken)
    {
        await _publisher.PublishEmployeeCreated(notification.Email, notification.EmployeeId, cancellationToken);
    }
}
