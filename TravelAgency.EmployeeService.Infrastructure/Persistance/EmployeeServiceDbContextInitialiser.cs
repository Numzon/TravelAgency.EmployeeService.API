using Dapper;
using Microsoft.Extensions.Options;
using System.Data;
using TravelAgency.EmployeeService.Application.Common.Models;

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
        await InitialiseProceduresAsync(connection);
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
        var sql = $@"CREATE TABLE IF NOT EXISTS salary (
                        id SERIAL PRIMARY KEY,
                        ammount MONEY NOT NULL,
                        {GetAudiableProperties()}
                    );

                    CREATE TABLE IF NOT EXISTS employee (
                        id SERIAL PRIMARY KEY,
                        user_id VARCHAR,
                        first_name VARCHAR NOT NULL,
                        last_name VARCHAR NOT NULL,
                        email VARCHAR NOT NULL,
                        salary_id SERIAL REFERENCES salary(id) NOT NULL,
                        travel_agency_id SERIAL NOT NULL,
                        {GetAudiableProperties()}
                    );";

        await connection.ExecuteAsync(sql);
    }

    private async Task InitialiseProceduresAsync(IDbConnection connection)
    {
        //var builder = new StringBuilder();

        ////builder.Append("CREATE OR REPLACE PROCEDURE insert_employee_with_salary");
        ////builder.Append("(email varchar, first_name varchar, last_name varchar, travel_agency_id int, ammount money, created timestamp, created_by varchar)");
        ////builder.Append("LANGUAGE plpgsql ");
        ////builder.Append("AS $$ ");
        ////builder.Append("BEGIN ");
        ////builder.Append("INSERT INTO salary (ammount, created, created_by) VALUES (ammount, created, created_by);");
        ////builder.Append("INSERT INTO employee (first_name, last_name, email, salary_id, travel_agency_id, created, created_by) ");
        ////builder.Append("VALUES (first_name, last_name, email, ");
        ////builder.Append("currval(pg_get_serial_sequence('salary','id')), ");
        ////builder.Append("travel_agency_id, created, created_by); ");
        ////builder.Append("COMMIT; ");
        ////builder.Append("END; $$; ");

        //var query = builder.ToString();

        //await connection.ExecuteAsync(query);
    }

    private async Task InitialiseFunctionsAsync(IDbConnection connection)
    {
        var sql = $@"CREATE OR REPLACE FUNCTION insert_employee_with_salary
                        (email varchar, first_name varchar, last_name varchar, travel_agency_id int, ammount money, created timestamp, created_by varchar)
                    RETURNS int
                    LANGUAGE plpgsql
                    AS $$
                    DECLARE employee_id int;
                    BEGIN
                        INSERT INTO salary (ammount, created, created_by) 
                               VALUES (ammount, created, created_by);
                        INSERT INTO employee (first_name, last_name, email, salary_id, travel_agency_id, created, created_by)
                               VALUES (first_name, last_name, email, currval(pg_get_serial_sequence('salary','id')), travel_agency_id, created, created_by);

                        SELECT currval(pg_get_serial_sequence('employee','id')) INTO employee_id;

                        RETURN employee_id;
                    END; $$;";

        await connection.ExecuteAsync(sql);
    }

    private string GetAudiableProperties()
    {
        return $@"created TIMESTAMP, 
                  created_by VARCHAR, 
                  last_modified TIMESTAMP,
                  last_modified_by VARCHAR";
    }
}
