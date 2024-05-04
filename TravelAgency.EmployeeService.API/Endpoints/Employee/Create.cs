using TravelAgency.EmployeeService.Application.Employees.Commands;

namespace TravelAgency.EmployeeService.API.Endpoints.Employee;

public sealed class Create : EndpointBaseAsync.WithRequest<CreateEmployeeCommand>.WithResult<IResult>
{
    private readonly ISender _sender;

    public Create(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("api/employees")]
    [SwaggerOperation(Summary = "Create employee", Tags = new[] { "Employees" })]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public override async Task<IResult> HandleAsync([FromBody] CreateEmployeeCommand request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = await _sender.Send(request);

        return Results.Created($"api/employees/{result.Id}", result);
    }
}
    