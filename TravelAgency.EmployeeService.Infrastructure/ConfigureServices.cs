using Amazon;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Serilog;
using TravelAgency.EmployeeService.Application.Common.Models;
using TravelAgency.EmployeeService.Infrastructure.Configs;
using TravelAgency.EmployeeService.Infrastructure.Persistance;
using TravelAgency.EmployeeService.Infrastructure.Publishers;
using TravelAgency.EmployeeService.Infrastructure.Repositories;
using TravelAgency.EmployeeService.Infrastructure.Services;
using TravelAgency.SharedLibrary.AWS;
using TravelAgency.SharedLibrary.Models;
using TravelAgency.SharedLibrary.RabbitMQ;

namespace TravelAgency.EmployeeService.Infrastructure;
public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.Configure<DatabaseSettingsDto>(builder.Configuration.GetRequiredSection("Database"));
        services.AddSingleton<EmployeeServiceDbContext>();
        services.AddSingleton<IEmployeeServiceDbContext, EmployeeServiceDbContext>();
        services.AddScoped<EmployeeServiceDbContextInitialiser>();

        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.RegisterRepositories();
        services.RegisterServices();
        services.RegisterPublishers();

        services.Configure<AwsCognitoSettingsDto>(builder.Configuration.GetRequiredSection("AWS:Cognito"));

        services.Configure<RabbitMqSettingsDto>(builder.Configuration.GetRequiredSection("RabbitMQ"));

        var cognitoConfiguration = builder.Configuration.GetRequiredSection("AWS:Cognito").Get<AwsCognitoSettingsDto>()!;

        try
        {
            services.AddAuthenticationAndJwtConfiguration(cognitoConfiguration);
        }
        catch (Exception ex)
        {
            if (!builder.Environment.IsDevelopment())
            {
                Log.Error(ex.Message);
            }
        }

        var rabbitMqSettings = builder.Configuration.GetRequiredSection("RabbitMQ").Get<RabbitMqSettingsDto>()!;

        services.AddRabbitMqConfiguration(rabbitMqSettings);
        //try
        //{
        //    services.AddRabbitMqConfiguration(rabbitMqSettings);
        //}
        //catch (Exception ex)
        //{
        //    Log.Error(ex.Message);
        //}

        builder.Services.AddSingleton(EventStrategiesConfig.GetGlobalSettingsConfiguration());

        services.AddAuthorizationWithPolicies();

        return services;
    }

    private static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        return services;
    }

    private static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeService, DateTimeService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }

    private static IServiceCollection RegisterPublishers(this IServiceCollection services)
    {
        services.AddScoped<IEmployeePublisher, EmployeePublisher>();

        return services;
    }
}
