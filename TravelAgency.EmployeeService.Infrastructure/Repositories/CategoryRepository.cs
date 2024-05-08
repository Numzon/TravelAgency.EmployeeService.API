using Dapper;
using TravelAgency.EmployeeService.Application.Categories.Commands.CreateCategory;
using TravelAgency.EmployeeService.Domain.Entities;

namespace TravelAgency.EmployeeService.Infrastructure.Repositories;
public sealed class CategoryRepository : ICategoryRepository
{
    private readonly IEmployeeServiceDbContext _context;
    private readonly ICurrentUserService _userService;
    private readonly IDateTimeService _dateTimeService;

    public CategoryRepository(IEmployeeServiceDbContext context, ICurrentUserService userService, IDateTimeService dateTimeService)
    {
        _context = context;
        _userService = userService;
        _dateTimeService = dateTimeService;
    }

    public Task<int> CreateAsync(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();   

        var connection = _context.CreateConnection();

        Guard.Against.Null(connection);

        var parameters = new DynamicParameters();
        parameters.Add("name", request.Name);
        parameters.Add("created", _dateTimeService.Now);
        parameters.Add("created_by", _userService.Id);

        return connection.ExecuteScalarAsync<int>("SELECT * FROM insert_category (@name, @created::TIMESTAMP, @created_by)", parameters);
    }

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();   

        var connection = _context.CreateConnection();

        Guard.Against.Null(connection);

        return await connection.QueryAsync<Category>(@$"SELECT 
                        id AS {nameof(Category.Id)},
                        name AS {nameof(Category.Name)},
                        created AS {nameof(Category.Created)},
                        created_by AS {nameof(Category.CreatedBy)},
                        last_modified AS {nameof(Category.LastModified)},
                        last_modified_by AS {nameof(Category.LastModifiedBy)}  
                    FROM category;");
    }
}
