namespace TravelAgency.EmployeeService.Application.Employees.Commands;
public sealed class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.FirstName)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotEmpty();

        RuleFor(x => x.TravelAgencyId)
            .NotEmpty();

        RuleFor(x => x.Ammount)
            .NotEmpty();
    }
}
