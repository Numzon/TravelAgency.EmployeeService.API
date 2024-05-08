using Ardalis.GuardClauses;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using TravelAgency.EmployeeService.Application.Common.Interfaces;
using TravelAgency.EmployeeService.Application.Common.Models;
using TravelAgency.EmployeeService.Application.Employees.Commands.CreateEmployee;
using TravelAgency.EmployeeService.Tests.Shared.Configurations;
using TravelAgency.EmployeeService.Tests.Shared.Enums;

namespace TravelAgency.EmployeeService.API.IntegrationTests.Endpoints.Employees;

[Collection(CollectionDefinitions.IntergrationTestCollection)]
public sealed class CreateTests : BaseIntegrationTest
{
    [Fact]
    public async Task HandleAsync_InvalidCommandData_ThrowsValidationException()
    {
        var command = new CreateEmployeeCommand("", "", "", 0, 0, 0, null!, null!);

        using (TestServer = HostConfiguration.Build().Server)
        {
            await InitializeDatabaseAsync();

            using HttpClient httpClient = TestServer.CreateClient();

            //act
            HttpResponseMessage httpResponse = await httpClient.PostAsync($"/api/employees", new StringContent(JsonConvert.SerializeObject(command, new StringEnumConverter()), Encoding.UTF8, MediaTypeNames.Application.Json));

            //assess
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            var exceptionDto = JsonConvert.DeserializeObject<ValidationExceptionDto>(responseContent);

            exceptionDto.Should().NotBeNull();
            exceptionDto!.Message.Should().NotBeNullOrEmpty();
            exceptionDto!.Errors.Should().NotBeNull().And.HaveCountGreaterThan(0);

            var properties = typeof(CreateEmployeeCommand).Properties().Where(x => x.Name != "EqualityContract").Select(x => x.Name).ToList();
            properties.ForEach(x => exceptionDto.Errors.Keys.Contains(x).Should().BeTrue());

            //cleanup
            await ResetDatabaseAsync();
        }
    }

    [Fact]
    public async Task HandleAsync_ValidCommandData_EmployeeCreated()
    {
        var command = Fixture.Build<CreateEmployeeCommand>().With(x => x.Email, Fixture.Create<MailAddress>().Address).Create();

        using (TestServer = HostConfiguration.Build().Server)
        {
            await InitializeDatabaseAsync();

            using HttpClient httpClient = TestServer.CreateClient();
            using IServiceScope scope = TestServer.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<IEmployeeServiceDbContext>();

            Guard.Against.Null(context);

            //act
            HttpResponseMessage httpResponse = await httpClient.PostAsync($"/api/employees", new StringContent(JsonConvert.SerializeObject(command, new StringEnumConverter()), Encoding.UTF8, MediaTypeNames.Application.Json));

            //assess
            httpResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            var exceptionDto = JsonConvert.DeserializeObject<BaseEntityDto>(responseContent);

            exceptionDto.Should().NotBeNull();
            exceptionDto!.Id.Should().BeGreaterThan(0);

            var connection = context.CreateConnection();

            var exists = await connection.ExecuteScalarAsync<bool>("SELECT COUNT(1) FROM employee WHERE id = @id", new { id = exceptionDto.Id });

            exists.Should().BeTrue();

            //cleanup
            await ResetDatabaseAsync();
        }
    }
}
