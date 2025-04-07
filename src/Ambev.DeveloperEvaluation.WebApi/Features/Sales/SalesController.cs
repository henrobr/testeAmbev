using Ambev.DeveloperEvaluation.Application.Sales.Commands.Cancel;
using Ambev.DeveloperEvaluation.Application.Sales.Commands.Complete;
using Ambev.DeveloperEvaluation.Application.Sales.Commands.Create;
using Ambev.DeveloperEvaluation.Application.Sales.Commands.Delete;
using Ambev.DeveloperEvaluation.Application.Sales.Commands.Update;
using Ambev.DeveloperEvaluation.Application.Sales.Queries.GetById;
using Ambev.DeveloperEvaluation.Application.Sales.Queries.GetListAll;
using Ambev.DeveloperEvaluation.Domain.Models.SaleAggregate.DTOs;
using Ambev.DeveloperEvaluation.WebApi.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

/// <summary>
/// Controller for managing sales operation
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SalesController(IMediator mediator) : BaseController
{
    /// <summary>
    /// Creates a new sale based on the provided data.
    /// </summary>
    /// <param name="command">The command containing sale details.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>
    /// Returns a 201 Created response with the newly created sale ID if successful.
    /// Returns a 400 Bad Request with validation errors if the request is invalid.
    /// </returns>
    /// <response code="201">Sale created successfully.</response>
    /// <response code="400">Validation failed or sale creation unsuccessful.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<int>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateSaleCommand command,
        CancellationToken cancellationToken)
    {
        var validator = new CreateSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToApiResponse());

        return Ok(await mediator.Send(command, cancellationToken));
    }

    /// <summary>
    /// Retrieves a sale by its unique identifier.
    /// </summary>
    /// <param name="saleId">The unique identifier of the sale.</param>
    /// <param name="cancellationToken">Token to cancel the request if necessary.</param>
    /// <returns>
    /// Returns an HTTP 200 response with the sale details if found.  
    /// Returns HTTP 400 if the request is invalid.  
    /// Returns HTTP 404 if the sale is not found.
    /// </returns>
    /// <response code="200">Successfully retrieved the sale details.</response>
    /// <response code="400">Invalid sale ID provided.</response>
    /// <response code="404">Sale not found.</response>
    [HttpGet("{saleId}")]
    [ProducesResponseType(typeof(ApiResponseWithData<SaleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int saleId, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetByIdQuery(saleId), cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Retrieves a paginated list of sales based on optional filters.
    /// </summary>
    /// <param name="saleId">Optional. The unique identifier of a sale to filter by.</param>
    /// <param name="customerName">Optional. The name of the customer to filter by.</param>
    /// <param name="branchName">Optional. The name of the branch to filter by.</param>
    /// <param name="page">The page number for pagination (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <param name="cancellationToken">Token to cancel the request if necessary.</param>
    /// <returns>
    /// Returns an HTTP 200 response with a paginated list of sales.  
    /// Returns HTTP 400 if the request is invalid.
    /// </returns>
    /// <response code="200">Successfully retrieved the paginated list of sales.</response>
    /// <response code="400">Invalid request parameters.</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<SaleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll(int? saleId = null,
        string? customerName = null,
        string? branchName = null,
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = await mediator.Send(new GetAllSaleQuery(saleId, customerName, branchName));

        var response = await PaginatedList<SaleDto>
            .CreateAsync(query, page, pageSize, cancellationToken);

        return OkPaginated(response);
    }
    
    /// <summary>
    /// Updates an existing sale with the provided data.
    /// </summary>
    /// <param name="saleId">The unique identifier of the sale to update.</param>
    /// <param name="command">The command containing the updated sale data.</param>
    /// <param name="cancellationToken">Token to cancel the request if necessary.</param>
    /// <returns>
    /// Returns an HTTP 204 (No Content) response when the sale is successfully updated.  
    /// Returns HTTP 400 if the request data is invalid.  
    /// Returns HTTP 404 if the sale is not found.
    /// </returns>
    /// <response code="204">Sale updated successfully.</response>
    /// <response code="400">Invalid request parameters.</response>
    /// <response code="404">Sale not found.</response>
    [HttpPut("{saleId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] int saleId, [FromBody] UpdateSaleCommand command,
        CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleValidator(saleId);
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToApiResponse());

        var response = await mediator.Send(command with { SaleId = saleId }, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Complete a sale by updating its status.
    /// </summary>
    /// <param name="saleId">The ID of the sale to be completed.</param>
    /// <param name="command">The command containing the details for completing the sale.</param>
    /// <param name="cancellationToken">The cancellation token for the request.</param>
    /// <returns>
    /// Returns <see cref="NoContentResult"/> (204 No Content) if the sale is successfully completed.
    /// Returns <see cref="BadRequestResult"/> (400 Bad Request) if the request is invalid.
    /// Returns <see cref="NotFoundResult"/> (404 Not Found) if the sale does not exist.
    /// </returns>
    /// <response code="204">The sale was successfully completed.</response>
    /// <response code="400">The request contains validation errors.</response>
    /// <response code="404">The specified sale was not found.</response>
    [HttpPatch("complete/{saleId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Canplete(int saleId, CancellationToken cancellationToken)
    {
        var command = new CompleteSaleCommand(saleId);

        var validator = new CompleteSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToApiResponse());

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Cancels a sale by updating its status.
    /// </summary>
    /// <param name="saleId">The ID of the sale to be canceled.</param>
    /// <param name="command">The command containing the details for canceling the sale.</param>
    /// <param name="cancellationToken">The cancellation token for the request.</param>
    /// <returns>
    /// Returns <see cref="NoContentResult"/> (204 No Content) if the sale is successfully canceled.
    /// Returns <see cref="BadRequestResult"/> (400 Bad Request) if the request is invalid.
    /// Returns <see cref="NotFoundResult"/> (404 Not Found) if the sale does not exist.
    /// </returns>
    /// <response code="204">The sale was successfully canceled.</response>
    /// <response code="400">The request contains validation errors.</response>
    /// <response code="404">The specified sale was not found.</response>
    [HttpPatch("cancel/{saleId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(int saleId, CancellationToken cancellationToken)
    {
        var command = new CancelSaleCommand(saleId);

        var validator = new CancelSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToApiResponse());

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Deletes a sale by its unique identifier.
    /// </summary>
    /// <param name="saleId">The unique identifier of the sale to delete.</param>
    /// <param name="cancellationToken">Token to cancel the request if necessary.</param>
    /// <returns>
    /// Returns an HTTP 204 (No Content) response when the sale is successfully deleted.  
    /// Returns HTTP 400 if the request is invalid.  
    /// Returns HTTP 404 if the sale is not found.
    /// </returns>
    /// <response code="204">Sale deleted successfully.</response>
    /// <response code="400">Invalid request parameters.</response>
    /// <response code="404">Sale not found.</response>
    [HttpDelete("{saleId}")]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int saleId, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteSaleCommand(saleId));

        return NoContent();
    }
}
