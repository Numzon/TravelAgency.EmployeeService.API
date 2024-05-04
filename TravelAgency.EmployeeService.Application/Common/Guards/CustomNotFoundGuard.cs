using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using TravelAgency.EmployeeService.Application.Common.Exceptions;

namespace Ardalis.GuardClauses;

public static class CustomNotFoundGuard
{
    public static T CustomNotFound<TKey, T>(this IGuardClause guardClause,
        [NotNull][ValidatedNotNull] TKey key,
        [NotNull][ValidatedNotNull] T? input,
        [CallerArgumentExpression("input")] string? parameterName = null) where TKey : struct
    {
        guardClause.Null(key);

        if (input is null)
        {
            throw new CustomNotFoundException(key.ToString()!, parameterName!);
        }

        return input;
    }
}
