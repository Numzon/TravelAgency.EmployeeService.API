using TravelAgency.EmployeeService.Application.Common.Interfaces;
using TravelAgency.EmployeeService.Application.Common.Models;

namespace TravelAgency.EmployeeService.Application.Categories.Commands.CreateCategory;
public sealed record CreateCategoryCommand(string Name) : IRequest<BaseEntityDto>;

public sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, BaseEntityDto>
{
    private readonly ICategoryRepository _repository;

    public CreateCategoryCommandHandler(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<BaseEntityDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var id = await _repository.CreateAsync(request, cancellationToken);

        return new BaseEntityDto(id);
    }
}
