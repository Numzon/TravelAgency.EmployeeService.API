using Dapper;
using TravelAgency.EmployeeService.Application.Employees.Commands.CreateEmployee;
using TravelAgency.EmployeeService.Domain.Entities;
using TravelAgency.EmployeeService.Domain.ValueObjects;

namespace TravelAgency.EmployeeService.Infrastructure.Repositories;
public sealed class EmployeeRepository : IEmployeeRepository
{
    private readonly IEmployeeServiceDbContext _context;
    private readonly IDateTimeService _dateTimeService;
    private readonly ICurrentUserService _currentUserService;

    public EmployeeRepository(IEmployeeServiceDbContext context, IDateTimeService dateTimeService, ICurrentUserService currentUserService)
    {
        _context = context;
        _dateTimeService = dateTimeService;
        _currentUserService = currentUserService;
    }

    public async Task UpdateUserIdAsync(int employeeId, string userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var exists = await CheckIfEmployeeExists(employeeId);

        if (exists is false)
        {
            throw new NotFoundException(employeeId.ToString(), "Employee");
        }

        using var connection = _context.CreateConnection();

        var sql = "UPDATE employee SET user_id = @UserId WHERE id = @Id";
        await connection.ExecuteAsync(sql, new { userId, id = employeeId });
    }

    public async Task<Employee?> GetEmployeeAsync(int employeeId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using var connection = _context.CreateConnection();

        var sql = $@"SELECT first_name AS {nameof(PersonalData.FirstName)}, 
                            last_name AS {nameof(PersonalData.LastName)}, 
                            email AS {nameof(PersonalData.Email)},
                            id AS {nameof(Employee.Id)},
                            user_id AS {nameof(Employee.UserId)},
                            travel_agency_id AS {nameof(Employee.TravelAgencyId)},
                            employee_type {nameof(Employee.EmployeeType)},
                            custom_employee_type {nameof(Employee.CustomEmployeeType)},
                            driving_licence_id AS {nameof(Employee.DrivingLicenceId)},
                            salary_id AS {nameof(Employee.SalaryId)},
                            created AS {nameof(Employee.Created)},
                            created_by AS {nameof(Employee.CreatedBy)},
                            last_modified AS {nameof(Employee.LastModified)},
                            last_modified_by AS {nameof(Employee.LastModifiedBy)}
                    FROM employee 
                    WHERE id = @Id";

        var response = await connection.QueryAsync<PersonalData, Employee, Employee>(sql,
            (p, e) =>
            {
                e.PersonalData = p;
                return e;
            }, splitOn: "id", param: new { id = employeeId });

        return response.FirstOrDefault();
    }

    public async Task<int> CreateAsync(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        using var connection = _context.CreateConnection();

        var paramenters = new DynamicParameters();
        paramenters.Add("email", request.Email);
        paramenters.Add("first_name", request.FirstName);
        paramenters.Add("last_name", request.LastName);
        paramenters.Add("travel_agency_id", request.TravelAgencyId);
        paramenters.Add("ammount", request.Ammount);
        paramenters.Add("employee_type", request.EmployeeType);
        paramenters.Add("custom_employee_type", request.CustomEmployeeType);
        paramenters.Add("identifier", request.Identifier);
        paramenters.Add("category_ids", request.CategoryIds);
        paramenters.Add("created", _dateTimeService.Now);
        paramenters.Add("created_by", _currentUserService.Id);

        var sql = "select * from insert_employee_data (@email, @first_name, @last_name, @travel_agency_id, @ammount::money, @employee_type, @custom_employee_type, @identifier, @category_ids, @created::timestamp, @created_by);";
        var employeeId = await connection.ExecuteScalarAsync<int>(sql, paramenters);

        return employeeId;
    }

    private async Task<bool> CheckIfEmployeeExists(int employeeId)
    {
        using var connection = _context.CreateConnection();

        var sql = "SELECT COUNT(1) FROM employee WHERE id = @Id";
        return await connection.ExecuteScalarAsync<bool>(sql, new { id = employeeId });
    }
}
