using TravelAgency.EmployeeService.Application.Common.Interfaces;
using TravelAgency.EmployeeService.Application.Common.Models;

namespace TravelAgency.EmployeeService.Application.Employees.Queries.GetEmployee;
public sealed record GetEmployeeQuery(int Id) : IRequest<EmployeeDto>;

public sealed class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, EmployeeDto>
{
    private readonly IEmployeeRepository _repository;

    public GetEmployeeQueryHandler(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<EmployeeDto> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
    {
        var employee = await _repository.GetEmployeeAsync(request.Id, cancellationToken);

        Guard.Against.CustomNotFound(request.Id, employee);

        return employee.Adapt<EmployeeDto>();
    }
}
