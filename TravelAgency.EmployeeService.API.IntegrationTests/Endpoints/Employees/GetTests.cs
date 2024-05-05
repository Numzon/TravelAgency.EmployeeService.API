using Ardalis.GuardClauses;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using TravelAgency.EmployeeService.Application.Common.Interfaces;
using TravelAgency.EmployeeService.Application.Common.Models;
using TravelAgency.EmployeeService.Application.Employees.Queries.GetEmployee;
using TravelAgency.EmployeeService.Tests.Shared.Configurations;
using TravelAgency.EmployeeService.Tests.Shared.Enums;

namespace TravelAgency.EmployeeService.API.IntegrationTests.Endpoints.Employees;

[Collection(CollectionDefinitions.IntergrationTestCollection)]
public sealed class GetTests : BaseIntegrationTest
{
    [Fact]
    public async Task HandleAsync_InvalidQuery_ThrowsValidationException()
    {
        using (TestServer = HostConfiguration.Build().Server)
        {
            await InitializeDatabaseAsync();

            using HttpClient httpClient = TestServer.CreateClient();

            //act
            HttpResponseMessage httpResponse = await httpClient.GetAsync($"/api/employees/{0}");

            //assess
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            var exceptionDto = JsonConvert.DeserializeObject<ValidationExceptionDto>(responseContent);

            exceptionDto.Should().NotBeNull();
            exceptionDto!.Message.Should().NotBeNullOrEmpty();
            exceptionDto!.Errors.Should().NotBeNull().And.HaveCountGreaterThan(0);

            var properties = typeof(GetEmployeeQuery).Properties().Where(x => x.Name != "EqualityContract").Select(x => x.Name).ToList();
            properties.ForEach(x => exceptionDto.Errors.Keys.Contains(x).Should().BeTrue());

            //cleanup
            await ResetDatabaseAsync();
        }
    }

    [Fact]
    public async Task HandleAsync_EmplyeeDoesntExist_ThrowsCustomNotFoundExceptionAndReturnsExceptionDto()
    {
        using (TestServer = HostConfiguration.Build().Server)
        {
            await InitializeDatabaseAsync();

            using HttpClient httpClient = TestServer.CreateClient();

            //act
            HttpResponseMessage httpResponse = await httpClient.GetAsync($"/api/employees/{Fixture.Create<int>()}");

            //assess
            httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            var exceptionDto = JsonConvert.DeserializeObject<ExceptionDto>(responseContent);

            exceptionDto.Should().NotBeNull();
            exceptionDto!.Message.Should().NotBeNullOrEmpty();

            //cleanup
            await ResetDatabaseAsync();
        }
    }

    [Fact]
    public async Task HandleAsync_ValidId_ReturnsEmployee()
    {
        using (TestServer = HostConfiguration.Build().Server)
        {
            await InitializeDatabaseAsync();

            using HttpClient httpClient = TestServer.CreateClient();
            using IServiceScope scope = TestServer.Services.CreateScope();

            var context = scope.ServiceProvider.GetService<IEmployeeServiceDbContext>();

            Guard.Against.Null(context);

            var connection = context.CreateConnection();

            Guard.Against.Null(connection);

            var parameters = new DynamicParameters();
            parameters.Add("email", Fixture.Create<string>());
            parameters.Add("first_name", Fixture.Create<string>());
            parameters.Add("last_name", Fixture.Create<string>());
            parameters.Add("travel_agency_id", Fixture.Create<int>());
            parameters.Add("ammount", Fixture.Create<decimal>());
            parameters.Add("created", Fixture.Create<DateTime>());
            parameters.Add("created_by", Fixture.Create<string>());

            var employeeId = await connection.ExecuteScalarAsync<int>(@"select * from insert_employee_with_salary (@email, @first_name, @last_name, 
                                        @travel_agency_id, @ammount::money, @created::timestamp, @created_by);", parameters);
            var employee = await connection.QueryFirstOrDefaultAsync("select * from employee where id = @id", new { id = employeeId });
            var salary = await connection.QueryFirstOrDefaultAsync("select * from salary where id = @id", new { id = employee!.salary_id }); 

            //act
            HttpResponseMessage httpResponse = await httpClient.GetAsync($"/api/employees/{employeeId}");

            //assess
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            var employeeDto = JsonConvert.DeserializeObject<EmployeeDto>(responseContent);

            employeeDto.Should().NotBeNull();
            employeeDto!.Id.Should().Be(employeeId);
            employeeDto.FirstName.Should().Be(employee.first_name);
            employeeDto.LastName.Should().Be(employee.last_name);
            employeeDto.Email.Should().Be(employee.email);
            employeeDto.UserId.Should().Be(employee.user_id);
            employeeDto.TravelAgencyId.Should().Be(employee.travel_agency_id);
            employeeDto.SalaryId.Should().Be(employee.salary_id);
            employeeDto.SalaryId.Should().Be(salary!.id);
            parameters.Get<decimal>("ammount").Should().Be(salary.ammount); 

            //cleanup
            await ResetDatabaseAsync();
        }
    }
}
