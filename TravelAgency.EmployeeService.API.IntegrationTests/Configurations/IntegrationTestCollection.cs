using TravelAgency.EmployeeService.Tests.Shared.Configurations;
using TravelAgency.EmployeeService.Tests.Shared.Enums;

namespace TravelAgency.EmployeeService.API.IntegrationTests.Configurations;

[CollectionDefinition(CollectionDefinitions.IntergrationTestCollection)]
public class IntegrationTestCollection : ICollectionFixture<TestContainerConfiguration>
{
    //configuration, no code here, class will not be ever created
}
