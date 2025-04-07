using Ambev.DeveloperEvaluation.Application.Products.Commands.Create;
using Ambev.DeveloperEvaluation.Application.Products.Queries.GetAll;
using Ambev.DeveloperEvaluation.Domain.Models.ProductAggregate.DTOs;
using Ambev.DeveloperEvaluation.WebApi.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products;

/// <summary>
/// Controller for managing products operation
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ProductsController(IMediator mediator) : BaseController
{
    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="command">The command containing the product data to be created.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>
    /// Returns <see cref="ApiResponseWithData{int}"/> with the ID of the created product  
    /// or <see cref="ApiResponse"/> with validation errors.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<int>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        var validator = new CreateProductValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToApiResponse());

        return Ok(await mediator.Send(command, cancellationToken));
    }

    /// <summary>
    /// Retrieves a paginated list of products, optionally filtered by name.
    /// </summary>
    /// <param name="name">Optional name filter to search products by name.</param>
    /// <param name="page">The page number (default is 1).</param>
    /// <param name="pageSize">The number of products per page (default is 10).</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>
    /// Returns <see cref="ApiResponseWithData{ProductDto}"/> with a paginated list of products  
    /// or <see cref="ApiResponse"/> if a bad request occurs.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll(
        string? name = null,
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = await mediator.Send(new GetAllProductQuery(name));

        var response = await PaginatedList<ProductDto>
            .CreateAsync(query, page, pageSize, cancellationToken);

        return OkPaginated(response);
    }
}
