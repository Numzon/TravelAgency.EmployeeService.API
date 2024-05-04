using TravelAgency.EmployeeService.Application.Common.Interfaces;
using TravelAgency.EmployeeService.Application.Common.Models;

namespace TravelAgency.EmployeeService.Application.Employees.Commands;
public sealed record CreateEmployeeCommand(string Email, string FirstName, string LastName, int TravelAgencyId, decimal Ammount) : IRequest<BaseEntityDto>;

public sealed class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, BaseEntityDto>
{
    private readonly IEmployeeRepository _repository;

    public CreateEmployeeCommandHandler(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<BaseEntityDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var id = await _repository.CreateAsync(request, cancellationToken);

        return new BaseEntityDto(id);
    }
}
