using System.Reflection;
using TravelAgency.EmployeeService.API.Middlewares;
using TravelAgency.EmployeeService.Application;
using TravelAgency.EmployeeService.Infrastructure;
using TravelAgency.EmployeeService.Infrastructure.Persistance;
using TravelAgency.SharedLibrary.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
if (builder.Environment.IsDevelopment())
{
    Assembly assembly = typeof(Program).Assembly;
    builder.Services.AddAndConfigureSwagger(assembly.GetName().Name!);
}

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
       .UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var initialiser = scope.ServiceProvider.GetRequiredService<EmployeeServiceDbContextInitialiser>();
        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();

#pragma warning disable S1118 // Utility classes should not have public constructors
public partial class Program { }
#pragma warning restore S1118 // Utility classes should not have public constructors