using TravelAgency.EmployeeService.Application.Categories.Queries.ListCategory;

namespace TravelAgency.EmployeeService.API.Endpoints.Categories;

public sealed class List : EndpointBaseAsync.WithoutRequest.WithResult<IResult>
{
    private readonly ISender _sender;

    public List(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("api/categories")]
    [SwaggerOperation(Summary = "Gets categories", Tags = new[] { "Categories" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public override async Task<IResult> HandleAsync(CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new ListCategoryQuery(), cancellationToken);

        return Results.Ok(result);
    }
}
