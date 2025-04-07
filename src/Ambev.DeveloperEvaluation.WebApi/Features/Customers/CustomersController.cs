using Ambev.DeveloperEvaluation.Application.Customers.Commands.Create;
using Ambev.DeveloperEvaluation.Application.Customers.Queries.GetAll;
using Ambev.DeveloperEvaluation.Domain.Models.CustomerAggregate.DTOs;
using Ambev.DeveloperEvaluation.WebApi.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers;

/// <summary>
/// Controller for managing customers operation
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class CustomersController(IMediator mediator) : BaseController
{
    /// <summary>
    /// Creates a new customer.
    /// </summary>
    /// <param name="command">The command containing customer data to be created.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>
    /// Returns <see cref="ApiResponseWithData{Guid}"/> with the ID of the created customer if successful,  
    /// <see cref="ApiResponse"/> with validation errors if the input is invalid.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCustomerCommand command,
        CancellationToken cancellationToken)
    {
        var validator = new CreateCustomerValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToApiResponse());

        return Ok(await mediator.Send(command, cancellationToken));
    }

    /// <summary>
    /// Retrieves a paginated list of customers, optionally filtered by name.
    /// </summary>
    /// <param name="name">Optional name filter to search customers by name.</param>
    /// <param name="page">The current page number (default is 1).</param>
    /// <param name="pageSize">The number of records per page (default is 10).</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>
    /// Returns <see cref="ApiResponseWithData{CustomerDto}"/> with a paginated list of customers  
    /// or <see cref="ApiResponse"/> with a bad request status if an error occurs.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<CustomerDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll(
        string? name = null,
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = await mediator.Send(new GetAllCustomerQuery(name));

        var response = await PaginatedList<CustomerDto>
            .CreateAsync(query, page, pageSize, cancellationToken);

        return OkPaginated(response);
    }
}
