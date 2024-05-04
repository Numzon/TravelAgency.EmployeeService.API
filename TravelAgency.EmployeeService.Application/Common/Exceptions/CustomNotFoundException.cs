using Microsoft.AspNetCore.Http;
using System.Runtime.Serialization;
using TravelAgency.EmployeeService.Application.Common.Interfaces;
using TravelAgency.EmployeeService.Application.Common.Models;

namespace TravelAgency.EmployeeService.Application.Common.Exceptions;

[Serializable]
public sealed class CustomNotFoundException : Exception, IExceptionStrategy
{
    private readonly int _statusCode;

    public CustomNotFoundException(string key, string objectName)
        : base($"Queried object {objectName} was not found, Key: {key}")
    {
    }

    public CustomNotFoundException(string key, string objectName, Exception innerException)
        : base($"Queried object {objectName} was not found, Key: {key}", innerException)
    {
    }

    private CustomNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        _statusCode = StatusCodes.Status404NotFound;
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue("statusCode", _statusCode);
    }

    public async Task ModifyAndWriteAsJsonAsync(HttpResponse response)
    {
        response.ContentType = "application/json";
        response.StatusCode = StatusCodes.Status404NotFound;
        await response.WriteAsJsonAsync(new ExceptionDto
        {
            Message = Message
        });
    }
}
