using TravelAgency.EmployeeService.Application.Categories.Commands.CreateCategory;

namespace TravelAgency.EmployeeService.API.Endpoints.Categories;

public sealed class Create : EndpointBaseAsync.WithRequest<CreateCategoryCommand>.WithResult<IResult>
{
    private readonly ISender _sender;

    public Create(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("api/categories")]
    [SwaggerOperation(Summary = "Creates category", Tags = new[] { "Categories" })]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public override async Task<IResult> HandleAsync([FromBody] CreateCategoryCommand request, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(request);

        return Results.Created($"api/categories/{result.Id}", result);
    }
}
