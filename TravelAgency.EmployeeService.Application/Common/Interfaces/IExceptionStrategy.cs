using Microsoft.AspNetCore.Http;

namespace TravelAgency.EmployeeService.Application.Common.Interfaces;
public interface IExceptionStrategy
{
    Task ModifyAndWriteAsJsonAsync(HttpResponse response);
}
