using TravelAgency.EmployeeService.Application.Common.Interfaces;
using TravelAgency.EmployeeService.Application.Common.Models;
using TravelAgency.EmployeeService.Domain.Enums;
using TravelAgency.EmployeeService.Domain.Events;

namespace TravelAgency.EmployeeService.Application.Employees.Commands.CreateEmployee;
public sealed record CreateEmployeeCommand(string Email, string FirstName, string LastName,
    int TravelAgencyId, decimal Ammount, int EmployeeType, string? CustomEmployeeType,
     string? Identifier = null, List<int>? CategoryIds = null) : IRequest<BaseEntityDto>;

public sealed class CreateEmployeeCommandHanler : IRequestHandler<CreateEmployeeCommand, BaseEntityDto>
{
    private readonly IEmployeeRepository _repository;
    private readonly IPublisher _publisher;

    public CreateEmployeeCommandHanler(IEmployeeRepository repository, IPublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task<BaseEntityDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var id = await _repository.CreateAsync(request, cancellationToken);

        //if (request.EmployeeType == EmployeeTypes.Driver.Value)
        //{
        //    //publish driver
        //}

        var emploteeEvent = new EmployeeCreatedEvent
        {
            Email = request.Email,
            EmployeeId = id
        };

        await _publisher.Publish(emploteeEvent, cancellationToken);

        return new BaseEntityDto(id);
    }
}
