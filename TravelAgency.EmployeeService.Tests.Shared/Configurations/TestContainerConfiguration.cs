using DotNet.Testcontainers.Containers;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace TravelAgency.EmployeeService.Tests.Shared.Configurations;
public class TestContainerConfiguration : IDisposable
{
    public IContainer PostgreSqlDatabase { get; private set; }
    public IContainer RabbitMqContainer { get; private set; }

    public TestContainerConfiguration()
    {
        PostgreSqlDatabase = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithName("PostgresSqlTests")
            .WithPortBinding(5100, 5432)
            .WithDatabase("postgres")
            .WithUsername("postgres")
            .WithPassword("test.1234")
            .Build();

        RabbitMqContainer = new RabbitMqBuilder()
            .WithImage("rabbitmq:3-management")
            .WithName("RabbitMqTests")
            .WithPortBinding(15600, 15672)
            .WithPortBinding(5600, 5672)
            .WithUsername("admin")
            .WithPassword("test.1234")
            .Build();

        PostgreSqlDatabase.StartAsync().Wait();
        RabbitMqContainer.StartAsync().Wait();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            PostgreSqlDatabase.StopAsync().Wait();
            RabbitMqContainer.StopAsync().Wait();
        }
    }
}