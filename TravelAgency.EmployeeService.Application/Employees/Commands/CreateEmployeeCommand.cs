using TravelAgency.EmployeeService.Application.Common.Interfaces;
using TravelAgency.EmployeeService.Application.Common.Models;
using TravelAgency.EmployeeService.Domain.Events;

namespace TravelAgency.EmployeeService.Application.Employees.Commands;
public sealed record CreateEmployeeCommand(string Email, string FirstName, string LastName, int TravelAgencyId, decimal Ammount) : IRequest<BaseEntityDto>;

public sealed class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, BaseEntityDto>
{
    private readonly IEmployeeRepository _repository;
    private readonly IPublisher _publisher;

    public CreateEmployeeCommandHandler(IEmployeeRepository repository, IPublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task<BaseEntityDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var id = await _repository.CreateAsync(request, cancellationToken);

        var emploteeEvent = new EmployeeCreatedEvent
        {
            Email = request.Email,
            EmployeeId = id
        };

        await _publisher.Publish(emploteeEvent, cancellationToken);

        return new BaseEntityDto(id);
    }
}
