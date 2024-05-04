namespace TravelAgency.EmployeeService.Application.Employees.Queries.GetEmployee;
public sealed class GetEmployeeQueryValidator : AbstractValidator<GetEmployeeQuery>
{
    public GetEmployeeQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
