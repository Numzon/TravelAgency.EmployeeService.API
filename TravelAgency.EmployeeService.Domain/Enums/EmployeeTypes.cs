using Ardalis.SmartEnum.Dapper;

namespace TravelAgency.EmployeeService.Domain.Enums;
public sealed class EmployeeTypes : DapperSmartEnumByValue<EmployeeTypes>
{
    public static readonly EmployeeTypes Driver = new EmployeeTypes("Driver", 1);
    public static readonly EmployeeTypes Receptionist = new EmployeeTypes("Receptionist", 2);
    public static readonly EmployeeTypes Cleaner = new EmployeeTypes("Cleaner", 3);
    public static readonly EmployeeTypes Custom = new EmployeeTypes("Custom", 4);

    public EmployeeTypes(string name, int value) : base(name, value)
    {
    }
}
