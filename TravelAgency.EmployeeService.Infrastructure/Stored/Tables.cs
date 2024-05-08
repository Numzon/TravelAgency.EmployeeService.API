namespace TravelAgency.EmployeeService.Infrastructure.Stored;
public static class Tables
{
    private const string AudiableProperties =
        @"created TIMESTAMP, 
            created_by VARCHAR, 
            last_modified TIMESTAMP,
            last_modified_by VARCHAR";

    public const string Salary =
        @$"CREATE TABLE IF NOT EXISTS salary (
            id SERIAL PRIMARY KEY,
            ammount MONEY NOT NULL,
            {AudiableProperties}
        );";

    public const string Category =
        @$"CREATE TABLE IF NOT EXISTS category (
            id SERIAL PRIMARY KEY,
            name VARCHAR NOT NULL,
            {AudiableProperties}
        );";
    public const string DrivingLicence =
        @$"CREATE TABLE IF NOT EXISTS driving_licence (
            id SERIAL PRIMARY KEY,
            identifier VARCHAR NOT NULL,
            {AudiableProperties}
        );";

    public const string DrivingLicenceCategory =
        $@"CREATE TABLE IF NOT EXISTS driving_licence_category (
            id SERIAL PRIMARY KEY,
            driving_licence_id SERIAL REFERENCES driving_licence(id) NOT NULL,
            category_id SERIAL REFERENCES category(id) NOT NULL,
            {AudiableProperties}
        );";

    public const string Employee =
        $@"CREATE TABLE IF NOT EXISTS employee (
            id SERIAL PRIMARY KEY,
            user_id VARCHAR,
            first_name VARCHAR NOT NULL,
            last_name VARCHAR NOT NULL,
            email VARCHAR NOT NULL,
            salary_id SERIAL REFERENCES salary(id) NOT NULL,
            travel_agency_id SERIAL NOT NULL,
            employee_type INTEGER NOT NULL,
            custom_employee_type VARCHAR,
            driving_licence_id INTEGER REFERENCES driving_licence(id),
            {AudiableProperties}
        );";
}
