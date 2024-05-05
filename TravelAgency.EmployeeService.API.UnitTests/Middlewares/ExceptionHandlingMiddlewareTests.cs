using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using TravelAgency.EmployeeService.API.Middlewares;
using TravelAgency.EmployeeService.Application.Common.Exceptions;
using TravelAgency.EmployeeService.Application.Common.Models;

namespace TravelAgency.EmployeeService.API.UnitTests.Middlewares;
public sealed class ExceptionHandlingMiddlewareTests
{
    private readonly Fixture _fixture;
    public ExceptionHandlingMiddlewareTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public async Task InvokeAsync_DefaultException_ThrowsException()
    {
        //arrange
        var message = _fixture.Create<string>();
        var context = new DefaultHttpContext();
        var next = new RequestDelegate((HttpContext httpContext) =>
        {
            throw new ArgumentException(message);
        });

        var middleware = new ExceptionHandlingMiddleware(next);

        //act and assess
        await middleware.Invoking(x => x.InvokeAsync(context)).Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task InvokeAsync_CustomNotFoundException_ReturnsHttpResponseWith404NotFound()
    {
        //arrange
        var context = new DefaultHttpContext();
        var next = new RequestDelegate((HttpContext httpContext) =>
        {
            throw new CustomNotFoundException("id", "randomObject");
        });

        var middleware = new ExceptionHandlingMiddleware(next);

        //act
        await middleware.InvokeAsync(context);

        //assess
        context.Response.Should().NotBeNull();
        context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task InvokeAsync_FluentValidationException_ReturnsHttpResponseWith400BadRequest()
    {
        //arrange
        var context = new DefaultHttpContext();
        var next = new RequestDelegate((HttpContext httpContext) =>
        {
            throw new FluentValidationException(_fixture.CreateMany<ValidationFailure>(10));
        });

        var middleware = new ExceptionHandlingMiddleware(next);

        //act
        await middleware.InvokeAsync(context);

        //assess
        context.Response.Should().NotBeNull();
        context.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }
}
