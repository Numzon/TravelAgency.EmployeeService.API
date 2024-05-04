using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TravelAgency.EmployeeService.Application.Common.Behaviours;
using TravelAgency.EmployeeService.Application.Mapster;

namespace TravelAgency.EmployeeService.Application;
public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.RegisterMapsterConfiguration();
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        });

        return services;
    }
}
