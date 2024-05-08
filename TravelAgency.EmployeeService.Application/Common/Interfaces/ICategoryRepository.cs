using TravelAgency.EmployeeService.Application.Categories.Commands.CreateCategory;
using TravelAgency.EmployeeService.Domain.Entities;

namespace TravelAgency.EmployeeService.Application.Common.Interfaces;
public interface ICategoryRepository
{
    Task<int> CreateAsync(CreateCategoryCommand request, CancellationToken cancellationToken);
    Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken);
}
