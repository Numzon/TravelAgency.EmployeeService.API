using TravelAgency.EmployeeService.Infrastructure.EventStrategies;
using TravelAgency.SharedLibrary.Enums;
using TravelAgency.SharedLibrary.RabbitMQ;

namespace TravelAgency.EmployeeService.Infrastructure.Configs;
public static class EventStrategiesConfig
{
    public static TypeEventStrategyConfig GetGlobalSettingsConfiguration()
    {
        var config = TypeEventStrategyConfig.GlobalSetting;

        config.NewConfig<UserCreatedForEmployeeEventStrategy>(EventTypes.UserForEmployeeCreated);

        return config;
    }
}
