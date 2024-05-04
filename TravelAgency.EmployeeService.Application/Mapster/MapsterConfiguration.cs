using Microsoft.Extensions.DependencyInjection;
using TravelAgency.EmployeeService.Application.Common.Models;
using TravelAgency.EmployeeService.Domain.Entities;

namespace TravelAgency.EmployeeService.Application.Mapster;

public static class MapsterConfiguration
{
    public static IServiceCollection RegisterMapsterConfiguration(this IServiceCollection services)
    {
        TypeAdapterConfig<Employee, EmployeeDto>
            .NewConfig()
            .Map(dest => dest.FirstName, src => src.PersonalData.FirstName)
            .Map(dest => dest.LastName, src => src.PersonalData.LastName)
            .Map(dest => dest.Email, src => src.PersonalData.Email);

        return services;
    }
}
