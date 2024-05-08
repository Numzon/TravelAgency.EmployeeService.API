using Dapper;
using Microsoft.Extensions.Options;
using System.Data;
using TravelAgency.EmployeeService.Application.Common.Models;
using TravelAgency.EmployeeService.Infrastructure.Stored;

namespace TravelAgency.EmployeeService.Infrastructure.Persistance;
public sealed class EmployeeServiceDbContextInitialiser
{
    private readonly EmployeeServiceDbContext _context;
    private readonly DatabaseSettingsDto _settings;

    public EmployeeServiceDbContextInitialiser(EmployeeServiceDbContext context, IOptions<DatabaseSettingsDto> options)
    {
        _context = context;
        _settings = options.Value;
    }

    public async Task InitialiseAsync()
    {
        using (var defaultConnection = _context.CreateDefaultConnection())
        {
            Guard.Against.Null(defaultConnection);

            await InitialiseDatabaseAsync(defaultConnection);
        }

        using var connection = _context.CreateConnection();

        Guard.Against.Null(connection);

        await InitialiseTablesAsync(connection);
        await InitialiseFunctionsAsync(connection);
    }

    public Task SeedAsync()
    {
        using var connection = _context.CreateConnection();

        return Task.CompletedTask;
    }

    private async Task InitialiseDatabaseAsync(IDbConnection connection)
    {
        var sql = "SELECT COUNT(1) FROM pg_database WHERE datname=@DatabaseName";

        var parameters = new DynamicParameters();
        parameters.Add("DatabaseName", _settings.Database.ToLower(), DbType.String);

        var sqlDbCount = await connection.ExecuteScalarAsync<bool>(sql, parameters);

        if (sqlDbCount is false)
        {
            sql = $"CREATE DATABASE {_settings.Database.ToLower()}";
            await connection.ExecuteAsync(sql);
        }
    }

    private async Task InitialiseTablesAsync(IDbConnection connection)
    {
        var sql = @$"{Tables.Salary}
                     {Tables.Category}
                     {Tables.DrivingLicence}
                     {Tables.DrivingLicenceCategory}
                     {Tables.Employee}";

        await connection.ExecuteAsync(sql);
    }

    private async Task InitialiseFunctionsAsync(IDbConnection connection)
    {
        var sql = $@"{Functions.Insert.DrivingLicence}
                     {Functions.Insert.Employee}                    
                     {Functions.Insert.Category}";

        await connection.ExecuteAsync(sql);
    }
}
