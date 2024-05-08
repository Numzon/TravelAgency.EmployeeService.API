using TravelAgency.EmployeeService.Application.Common.Interfaces;
using TravelAgency.EmployeeService.Application.Common.Models;

namespace TravelAgency.EmployeeService.Application.Categories.Queries.ListCategory;
public sealed class ListCategoryQuery : IRequest<IEnumerable<LookupDto>>
{
}

public sealed class ListCategoryQueryHandler : IRequestHandler<ListCategoryQuery, IEnumerable<LookupDto>>
{
    private readonly ICategoryRepository _repository;

    public ListCategoryQueryHandler(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<LookupDto>> Handle(ListCategoryQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetAllAsync(cancellationToken);

        return result.Adapt<IEnumerable<LookupDto>>();  
    }
}
