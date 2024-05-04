using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;
using System.Data.Common;
using TravelAgency.EmployeeService.Application.Common.Interfaces;
using TravelAgency.EmployeeService.Application.Common.Models;

namespace TravelAgency.EmployeeService.Infrastructure.Persistance;
public sealed class EmployeeServiceDbContext : IEmployeeServiceDbContext
{
    private readonly string _connectionString;
    private readonly string _defaultConnectionString;

    public EmployeeServiceDbContext(IOptions<DatabaseSettingsDto> options)
	{
        var settings = options.Value;

        Guard.Against.Null(settings);

        _connectionString = BuildConnectionString(settings);
        _defaultConnectionString = BuildDefaultConnectionString(settings);
    }

    public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);

    /// <summary>
    /// Builds connection string to postgresSQL with default database name set as 'postgres'
    /// Made to handle many databases in one postgres container - should be used only in initialisation of database
    /// </summary>
    /// <returns>Connection string to default database</returns>
    public IDbConnection CreateDefaultConnection() => new NpgsqlConnection(_defaultConnectionString);

    private string BuildConnectionString(DatabaseSettingsDto settings)
    {
        return BuildConnectionStringWithGivenDatabaseName(settings, settings.Database);
    }
    private string BuildDefaultConnectionString(DatabaseSettingsDto settings)
    {
        return BuildConnectionStringWithGivenDatabaseName(settings, "postgres");
    }

    private string BuildConnectionStringWithGivenDatabaseName(DatabaseSettingsDto settings, string databaseName)
    {
        var connectionStringBuilder = new DbConnectionStringBuilder
        {
            { "User ID", settings.UserId },
            { "Password", settings.Password },
            { "Host", settings.Host },
            { "Port", settings.Port },
            { "Database", databaseName },
            { "Pooling", settings.Pooling },
            { "Connection Lifetime", settings.ConnectionLifetime }
        };

        return connectionStringBuilder.ToString();
    }
}
