using TravelAgency.EmployeeService.Domain.Enums;

namespace TravelAgency.EmployeeService.Application.Employees.Commands.CreateEmployee;
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

        RuleFor(x => x.EmployeeType)
            .NotEmpty();

        RuleFor(x => x.CustomEmployeeType)
            .NotEmpty().When(x => x.EmployeeType == EmployeeTypes.Custom.Value);

        When(x => x.EmployeeType == EmployeeTypes.Driver.Value, () =>
        {
            RuleFor(x => x.Identifier)
                .NotEmpty();

            RuleFor(x => x.CategoryIds)
                .NotEmpty();
        });
    }
}
