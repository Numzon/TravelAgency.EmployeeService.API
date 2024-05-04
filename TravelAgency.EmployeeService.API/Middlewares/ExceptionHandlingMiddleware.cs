using Ardalis.GuardClauses;
using Serilog;
using TravelAgency.EmployeeService.Application.Common.Interfaces;

namespace TravelAgency.EmployeeService.API.Middlewares;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            Log.Error("Exception occurred: {Message}", exception.Message);

            if (exception is not IExceptionStrategy)
            {
                throw;
            }

            var strategy = exception as IExceptionStrategy;

            Guard.Against.Null(strategy);

            await strategy.ModifyAndWriteAsJsonAsync(context.Response);
        }
    }
}
