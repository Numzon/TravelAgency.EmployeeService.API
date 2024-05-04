namespace TravelAgency.EmployeeService.Application.Common.Models;
public class DatabaseSettingsDto
{
    public required string UserId { get; set; }
    public required string Password { get; set; }
    public required string Host { get; set; }
    public required string Port { get; set; }
    public required string Database { get; set; }
    public required string Pooling { get; set; }
    public required string ConnectionLifetime { get; set; }
}