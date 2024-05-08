using TravelAgency.EmployeeService.Application.Employees.Queries.GetEmployee;

namespace TravelAgency.EmployeeService.API.Endpoints.Employees;

public sealed class Get : EndpointBaseAsync.WithRequest<int>.WithResult<IResult>
{
    private readonly ISender _sender;

    public Get(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("api/employees/{id}")]
    [SwaggerOperation(Summary = "Gets employee", Tags = new[] { "Employees" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public override async Task<IResult> HandleAsync([FromRoute(Name = "id")]int request, 
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var employee = await _sender.Send(new GetEmployeeQuery(request), cancellationToken);

        return Results.Ok(employee);
    }
}
