using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using System.Runtime.Serialization;
using TravelAgency.EmployeeService.Application.Common.Interfaces;
using TravelAgency.EmployeeService.Application.Common.Models;

namespace TravelAgency.EmployeeService.Application.Common.Exceptions;

[Serializable]
public sealed class FluentValidationException : Exception, IExceptionStrategy
{
    private readonly int _statusCode;
    private readonly IDictionary<string, string[]> _errors;

    public FluentValidationException()
        : base("One or more validation failures have occurred")
    {
        _errors = new Dictionary<string, string[]>();
        _statusCode = StatusCodes.Status400BadRequest;
    }

    public FluentValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        _errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    private FluentValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        _statusCode = StatusCodes.Status404NotFound;
        _errors = new Dictionary<string, string[]>();
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue("statusCode", _statusCode);
        info.AddValue("validationErrors", _errors, typeof(IDictionary<string, string>));
    }

    public async Task ModifyAndWriteAsJsonAsync(HttpResponse response)
    {
        response.ContentType = "application/json";
        response.StatusCode = _statusCode;
        await response.WriteAsJsonAsync(new ValidationExceptionDto
        {
            Message = Message,
            Errors = _errors
        });
    }
}
