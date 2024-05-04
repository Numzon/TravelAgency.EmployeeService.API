using System.Data;

namespace TravelAgency.EmployeeService.Application.Common.Interfaces;
public interface IEmployeeServiceDbContext
{
    IDbConnection CreateConnection();
}
